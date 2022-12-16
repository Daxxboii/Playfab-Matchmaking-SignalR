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
    public static PlayFabManager instance;
    public static string EntityId,SessionTicket;
    string encryptedPassword;

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
        // Debug.Log(username.text);
        var registerRequest = new RegisterPlayFabUserRequest { Email = Email, Password = Encrypt(Password), Username = Username };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, RegisterSuccess, RegisterFailure);
    }

    void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        SessionTicket = result.SessionTicket;
        EntityId = result.EntityToken.Entity.Id;
        SceneManager.LoadScene("Gameplay");
    }

    void RegisterFailure(PlayFabError error)
    {
       UnityEngine.Debug.LogWarning(error);
    }

    public void Login(string Email, string Password)
    {
        var request = new LoginWithEmailAddressRequest { Email = Email, Password = Encrypt(Password), InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
        {
            GetPlayerProfile = true
        }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, LoginSuccess, loginFailure);
    }
    void loginFailure(PlayFabError error)
    {
      UnityEngine.Debug.LogWarning(error);
    }

    void LoginSuccess(LoginResult login)
    {
        SessionTicket = login.SessionTicket;
        EntityId = login.EntityToken.Entity.Id;
        Debug.Log("Logged In SuccessFully");
        SceneManager.LoadScene("Gameplay");
    }
    #endregion

}