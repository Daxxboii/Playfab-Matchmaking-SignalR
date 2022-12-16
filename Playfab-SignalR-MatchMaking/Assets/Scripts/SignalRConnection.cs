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
    private static Uri uri = new Uri("Localhost:5000/Gamehub");//To be replaced with your own uri

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
            Debug.Log(DeserializedData);
        });


    }

   public async void AddToQueue(string Queue){
    
        await connection.InvokeAsync<string>("AddToQueue", Queue);

   }


    public async void Disconnect(string Queue)
    {
        await connection.InvokeAsync<string>("RemoveFromQueue", Queue);
        await connection.StopAsync();
    }
}

