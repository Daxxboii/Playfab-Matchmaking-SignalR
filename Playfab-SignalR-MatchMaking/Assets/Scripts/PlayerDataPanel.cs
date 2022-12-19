using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MyBox;

public class PlayerDataPanel : MonoBehaviour
{
    public enum PlayerType{GlobalPlayer,Friend,FriendRequest}

    public PlayerType Type;
    [ReadOnly,OverrideLabel("Username(Read-Only)")]public string Username;

    public TextMeshProUGUI PlayerUsernameText,ButtonText;
    public Image OnlineStatus;
    public Button RequestButton;

    [ConditionalField(nameof(Type), false, PlayerType.FriendRequest)]public Button Accept,Decline;

    public bool IsOnline;

    void OnEnable(){
        Configure();
       
    }

    void Configure(){
        if(Type == PlayerType.GlobalPlayer)
       {
        RequestButton.onClick.AddListener(()=>RelayFriendRequestData("Sending"));
        //Accept.gameObject.SetActive(false);
        //Decline.gameObject.SetActive(false);
        
        RequestButton.gameObject.SetActive(true);

        ButtonText.text = "Send Friend Request";
       }

       else if(Type==PlayerType.Friend){
        //RequestButton.onClick.AddListener();
        //Accept.gameObject.SetActive(false);
        //Decline.gameObject.SetActive(false);
        
        RequestButton.gameObject.SetActive(true);

        ButtonText.text = "Add to party";

       }

       else if(Type==PlayerType.FriendRequest){
        //RequestButton.onClick.AddListener(SendFriendRequest);
        Accept.gameObject.SetActive(true);
        Decline.gameObject.SetActive(true);
        RequestButton.gameObject.SetActive(false);

        Accept.onClick.AddListener(()=>RelayFriendRequestData("Accepted"));
        Decline.onClick.AddListener(()=>RelayFriendRequestData("Declined"));
       } 

    }

    public void UpdatePanel()
    {
        PlayerUsernameText.text = Username;
        if (IsOnline) OnlineStatus.color = Color.green;
        else OnlineStatus.color = Color.red;
        Configure();
    }

    private void RelayFriendRequestData(string Status){
        if(Status == "Sending"){   
        SignalRConnection.instance.RelayFriendRequestData(PlayFabManager.PlayerUsername,this.Username,"Sending");
        RequestButton.interactable = false;
        }

        else if(Status == "Accepted")
        {
            PlayFabManager.instance.AddFriend(Username);
            SignalRConnection.instance.RelayFriendRequestData(PlayFabManager.PlayerUsername,this.Username,Status);
            Destroy(this.gameObject);
        }

        else if(Status == "Declined")
        {
            Destroy(this.gameObject);
        }
    }

    public void SetFriendRequestButtonState(bool state){
        RequestButton.interactable = state;
    }


}
