using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager instance; //Singleton

    public static string EntityId,SessionTicket,EntityToken;
    string encryptedPassword;

    public static string PlayerUsername;

    private void Awake()
    {
        if (instance == null) instance = this;
        DontDestroyOnLoad(this);
    }

    #region Signup and Login

    string Encrypt(string StringToEncrypt)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] bs = System.Text.Encoding.UTF8.GetBytes(StringToEncrypt);

        bs = x.ComputeHash(bs);
        System.Text.StringBuilder s = new System.Text.StringBuilder();

        foreach(byte b in bs)
        {
            s.Append(b.ToString("x2").ToLower());
        }
        return s.ToString();
    }

    public void SignUp(string Email , string Password,string Username )
    {
        var registerRequest = new RegisterPlayFabUserRequest { Email = Email, Password = Encrypt(Password), Username = Username,DisplayName = Username };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, RegisterSuccess, PlayFabErrorLog);
        PlayerUsername = Username;
    }

    void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        SessionTicket = result.SessionTicket;
        EntityId = result.EntityToken.Entity.Id;
        
        SignalRConnection.instance.RegisterOnline();
        SceneManager.LoadSceneAsync("Gameplay");
    }

   

    public void Login(string Email, string Password)
    {
        var request = new LoginWithEmailAddressRequest { Email = Email, Password = Encrypt(Password), InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
        {
            GetPlayerProfile = true
        }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, LoginSuccess, PlayFabErrorLog);
    }

    void LoginSuccess(LoginResult login)
    {
        SessionTicket = login.SessionTicket;
        EntityId = login.EntityToken.Entity.Id;
        EntityToken = login.EntityToken.EntityToken;
        Debug.Log("Logged In SuccessFully");
        PlayerUsername = login.InfoResultPayload.PlayerProfile.DisplayName;
        SignalRConnection.instance.RegisterOnline();
        SceneManager.LoadSceneAsync("Gameplay");
    }
    #endregion



    public void AddFriend(string name)
    {
        var request = new AddFriendRequest { FriendTitleDisplayName = name };
        PlayFabClientAPI.AddFriend(request, OnFriendAddedSuccess, PlayFabErrorLog);
    }

    void OnFriendAddedSuccess(AddFriendResult result) 
    { 
        Debug.Log("Added Friend!");
    }

    /// <summary>
    /// Fetches Friend List from PlayFab , enabel includesteamfriends to include steam friendlist
    /// </summary>
    public void GetFriendList(){
        PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest {
        IncludeSteamFriends = false,
        IncludeFacebookFriends = false,
        XboxToken = null
    }, result => {GameplayMenuManager.instance.WriteFriendList(result.Friends);}, PlayFabErrorLog);
    }
    void PlayFabErrorLog(PlayFabError error)
    {
        UnityEngine.Debug.LogError(error);
    }

}