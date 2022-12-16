using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.MultiplayerModels;
using MyBox;

public class MatchMaking : MonoBehaviour
{
    private string TicketID;
    private static string Queue = "Test_Queue"; // Can assign/create new queues , not to be hard coded , just hard coding for this example


    private Coroutine pollTicketCoroutine;

    private void Start()
    {
        StartMatchmaking();
    }

    public void StartMatchmaking()
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

                GiveUpAfterSeconds = 60,

                QueueName = Queue
            },
            OnMatchmakingTicketCreated,
            OnMatchmakingError
        );
    }

    private void OnMatchmakingTicketCreated(CreateMatchmakingTicketResult result)
    {
        TicketID = result.TicketId;
        pollTicketCoroutine = StartCoroutine(PollTicket(result.TicketId));

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
        
        SignalRConnection.instance.Disconnect(Queue);
    }
}