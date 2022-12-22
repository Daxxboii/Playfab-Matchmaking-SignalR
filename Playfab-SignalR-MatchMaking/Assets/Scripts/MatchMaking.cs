using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.MultiplayerModels;
using PlayFab.ClientModels;
using MyBox;
using EntityKey = PlayFab.MultiplayerModels.EntityKey;

public class MatchMaking : MonoBehaviour
{
    [HideInInspector]public string TicketID;
    private static string Queue = "Test_Queue"; // Can assign/create new queues , not to be hard coded , just hard coding for this example
    public static MatchMaking instance;

    /// <summary>
    /// Add the EntityKeys of the players in party to this list
    /// </summary>
    public static List<EntityKey> PlayersInQueue;

    private Coroutine pollTicketCoroutine;

    public static bool IsHost;


    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// To Call When Starting a Match
    /// </summary>
    public void StartMatchmaking()
    {
        if (IsHost)
        {
            PlayFabMultiplayerAPI.CreateMatchmakingTicket(
                new CreateMatchmakingTicketRequest
                {
                    Creator = new MatchmakingPlayer
                    {
                        Entity = new EntityKey
                        {
                            Id = PlayFabManager.EntityId,
                            Type = "title_player_account",
                        },
                        Attributes = new MatchmakingPlayerAttributes
                        {
                            DataObject = new { }
                        }
                    },
                    MembersToMatchWith = PlayersInQueue,

                    GiveUpAfterSeconds = 60,

                    QueueName = Queue
                },
                OnMatchmakingTicketCreated,
                OnMatchmakingError
            );
        }

        else
        {
            Debug.Log("Player is not host"); // Implies that player is in a playfabParty with someone else as the host , the player will enter matchmaking when the host starts and passes the ticket to this player via signalR
        }
    }

    public void JoinMatchmakingQueue()
    {
        PlayFabMultiplayerAPI.JoinMatchmakingTicket(new JoinMatchmakingTicketRequest
        {
            TicketId = TicketID,
            QueueName = Queue,
            Member = new MatchmakingPlayer
            {
                Entity = new EntityKey
                {
                    Id = "<Entity ID goes here>",
                    Type = "<Entity type goes here>",
                },
                Attributes = new MatchmakingPlayerAttributes
                {
                    DataObject = new
                    {
                        Skill = 19.3
                    },
                },
            }
        },
    this.OnJoinMatchmakingTicket,
    this.OnMatchmakingError);
    }
    private void OnMatchmakingTicketCreated(CreateMatchmakingTicketResult result)
    {
        TicketID = result.TicketId;
        pollTicketCoroutine = StartCoroutine(PollTicket(result.TicketId));

        SignalRConnection.instance.SendTicketID("AddUsernameofpartyfriends", TicketID);

        Debug.Log("MatchMaking Started");
    }

    private void OnJoinMatchmakingTicket(JoinMatchmakingTicketResult result)
    {
        pollTicketCoroutine = StartCoroutine(PollTicket(TicketID));
        Debug.Log("MatchMaking Started");
    }


    private IEnumerator PollTicket(string ticketId)
    {
        while (true)
        {
            Debug.Log("Still Looking for a match");
            PlayFabMultiplayerAPI.GetMatchmakingTicket(
                new GetMatchmakingTicketRequest
                {
                    TicketId = ticketId,
                    QueueName = Queue
                },
                OnGetMatchMakingTicket,
                OnMatchmakingError
            );

            yield return new WaitForSeconds(6);
        }
    }

    private void OnGetMatchMakingTicket(GetMatchmakingTicketResult result)
    {
        switch (result.Status)
        {
            case "Matched":
                StopCoroutine(pollTicketCoroutine);
                Debug.Log("Matched");
                StartMatch(result.MatchId);
                break;
            case "Canceled":
                StopCoroutine(pollTicketCoroutine);
                Debug.Log("Matchmaking Canceled");
                break;
        }
    }

    private void StartMatch(string matchId)
    {
        PlayFabMultiplayerAPI.GetMatch(
            new GetMatchRequest
            {
                MatchId = matchId,
                QueueName = Queue
            },
            OnGetMatch,
            OnMatchmakingError
        );
    }
    private void OnGetMatch(GetMatchResult result)
    {
        Debug.Log($"Match Started  {result.Members[0].Entity.Id} vs {result.Members[1].Entity.Id}");
        SignalRConnection.instance.AddToQueue(Queue);
    }

    private void OnMatchmakingError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
    [ButtonMethod]
    public void LeaveQueue()
    {
        PlayFabMultiplayerAPI.CancelMatchmakingTicket(
            new CancelMatchmakingTicketRequest
            {
                QueueName = Queue,
                TicketId = TicketID
            },
            OnTicketCanceled,
            OnMatchmakingError
        );
    }

    private void OnTicketCanceled(CancelMatchmakingTicketResult result)
    {
        Debug.Log("Ticket Cancelled");

    }
}