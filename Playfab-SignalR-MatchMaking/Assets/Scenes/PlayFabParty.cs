using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.Party;
using UnityEngine.Networking.Types;

using PlayFab.MultiplayerModels;

public class PlayFabParty : MonoBehaviour
{
    public static PlayFabParty instance;
    public static string NetworkID;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CreateNetwork()
    {

        PlayFabMultiplayerManager.Get().CreateAndJoinNetwork();
        PlayFabMultiplayerManager.Get().OnNetworkJoined += OnNetworkJoined;
        PlayFabMultiplayerManager.Get().OnRemotePlayerJoined += OnRemotePlayerJoined;
        PlayFabMultiplayerManager.Get().OnRemotePlayerLeft += OnRemotePlayerLeft;
    }

    public void JoinNetwork(string NetworkID)
    {
        PlayFabMultiplayerManager.Get().JoinNetwork(NetworkID);
        PlayFabMultiplayerManager.Get().OnNetworkJoined += OnNetworkJoined;
    }
    private void OnNetworkJoined(object sender, string networkId)
    {
        // Print the Network ID 
        Debug.Log(networkId);
        NetworkID = networkId;
    }

    private void OnRemotePlayerLeft(object sender, PlayFabPlayer player)
    {
        //Remove this player from Matchmaking.PlayersInQueue
    }

    private void OnRemotePlayerJoined(object sender, PlayFabPlayer player)
    {
        //Add this player to from Matchmaking.PlayersInQueue
    }

}
