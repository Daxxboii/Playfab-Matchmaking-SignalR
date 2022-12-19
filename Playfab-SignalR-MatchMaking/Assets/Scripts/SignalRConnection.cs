using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using TMPro;
using MyBox;
using PlayFab.Json;

public class SignalRConnection : MonoBehaviour
{ 
    // SignalR variables

    //Make Sure you replace the uri with your own
    private static Uri uri = new Uri("https://localhost:7002/Gamehub");//To be replaced with your own uri

    public static SignalRConnection instance;

    private HubConnection connection;

    //  Use this for initialization
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        Connect();
    }

    // Connect to the SignalR server
    public async void Connect()
    {
       connection = new HubConnectionBuilder().WithUrl(uri).Build();
       Debug.Log("Connecting to SignalR");

       await connection.StartAsync();

       Debug.Log("SignalR Connection Esthabished");


        //On Recieving Public Data
        connection.On<string>("GamePlayData", (data) =>
        {
            dynamic DeserializedData = JsonConvert.DeserializeObject(data);
            Debug.Log("List of players:");
        });
       
        connection.On<Dictionary<string,string>>("ListOfOnlinePlayers", (data) =>
        {
            Debug.Log("SuccessFully Updated List Of Online Players, " + data.Count + " Player(s) Online");
            GameplayMenuManager.OnlinePlayers = new List<string>();

            foreach (KeyValuePair<string, string> kvp in data)
            {
                GameplayMenuManager.OnlinePlayers.Add(kvp.Key);
            }
        });


        connection.On<Dictionary<string,string>>("FriendRequestData", (data) =>
        {
            foreach (KeyValuePair<string, string> kvp in data)
            {
                string key = kvp.Key;
                switch(kvp.Value){
                    case "Sending":
                    Debug.Log(key);
                    GameplayMenuManager.instance.WriteFriendRequests(key);
                    Debug.Log("FriendRequestData Received");
                    break;

                    case "Declined":

                    foreach(PlayerDataPanel panel in GameplayMenuManager.AllFriendRequests){
                        
                        GameplayMenuManager.AllFriendRequests.Remove(panel);
                        if(panel.Username==key)
                        {
                            Destroy(panel.gameObject);
                        }
                    }
                    break;

                    case "Accepted":
                    PlayFabManager.instance.AddFriend(key);
                    break;
                }
            }
        });
    }

   public async void AddToQueue(string Queue){
    
        await connection.InvokeAsync<string>("AddToQueue", Queue);

   }

   public async void RegisterOnline(){
        await connection.InvokeAsync<string>("RegisterPlayerOnline",PlayFabManager.PlayerUsername);
        Debug.Log("player Online");
   }


   public async void RelayFriendRequestData(string Username,string TargetUserName,string Status) // Make Sure Status is either set to Sending ,Declined or Accepted
   {
    await connection.InvokeAsync<string>("RelayFriendRequestData",Username,TargetUserName,Status);
   }

    [ButtonMethod] //For Testing in inspector panel
    public async void FetchAllOnlinePlayers()
    {
        await connection.InvokeAsync<string>("SendAllOnlinePlayers", PlayFabManager.PlayerUsername);
        GameplayMenuManager.instance.WriteGlobalList();
    }


    private void OnApplicationQuit(){
        Disconnect();
    }
    public async void Disconnect()
    {
        await connection.InvokeAsync<string>("DeRegisterPlayerOnline",PlayFabManager.PlayerUsername);
        Debug.Log("Player Offline");
        await connection.StopAsync();
    }
}

