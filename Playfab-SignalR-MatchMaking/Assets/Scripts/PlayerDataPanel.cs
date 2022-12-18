using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MyBox;

public class PlayerDataPanel : MonoBehaviour
{
    public enum PlayerType{GlobalPlayer,Friend}

    public PlayerType Type;
    [ReadOnly,OverrideLabel("Username(Read-Only)")]public string Username;

    public TextMeshProUGUI PlayerUsernameText;

    [ConditionalField(nameof(Type), false, PlayerType.GlobalPlayer)]public Button SendFriendRequestButton;
    [ConditionalField(nameof(Type), false, PlayerType.Friend)]public Button InviteToPartyButton;

    void OnEnable(){
        SendFriendRequestButton.onClick.AddListener(SendFriendRequest);
    }

    private void SendFriendRequest(){
        SignalRConnection.instance.SendFriendRequest(this.Username);
    }

    public void SetFriendRequestButtonState(bool state){
        SendFriendRequestButton.interactable = state;
    }


}
