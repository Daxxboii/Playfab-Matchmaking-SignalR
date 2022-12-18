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
            Debug.Log(DeserializedData);
        });

        connection.On<Dictionary<string,string>>("ListOfOnlinePlayers", (data) =>
        {
          //  dynamic DeserializeData = JsonConvert.DeserializeObject(data);
            Debug.Log(data);
        });

    }

   public async void AddToQueue(string Queue){
    
        await connection.InvokeAsync<string>("AddToQueue", Queue);

   }

   public async void RegisterOnline(){
        Debug.Log(PlayFabManager.PlayerUsername);
    await connection.InvokeAsync<string>("RegisterPlayerOnline",PlayFabManager.PlayerUsername);
        Debug.Log("player Online");
   }

   public async void SendFriendRequest(string Name){

   }

    [ButtonMethod]
    public async void FetchAllOnlinePlayers()
    {
        await connection.InvokeAsync<string>("SendAllOnlinePlayers", PlayFabManager.PlayerUsername);

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

