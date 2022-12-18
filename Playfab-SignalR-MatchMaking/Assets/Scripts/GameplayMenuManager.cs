using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;
public class GameplayMenuManager : MonoBehaviour
{
    [SerializeField,Foldout("Panels",true)]private GameObject HomePanel,FriendsPanel,MatchMakingPanel,GlobalPlayersPanel;
    
    [SerializeField, Foldout("Buttons", true)]
    private Button OpenFriendsPanelButton, OpenMatchMakingPanelButton,OpenGlobalPlayersButton;
    private Button[] BackButtons;

    [SerializeField, Foldout("Other", true)]
    private GameObject PlayerInfoPanelPrefab;


    void Start()
    {
        MapAllButtons();
        ShowHomePanel();
    }
    #region Mapping
    void MapAllButtons()
    {
        OpenFriendsPanelButton.onClick.AddListener(ShowFriendsPanel);
        OpenMatchMakingPanelButton.onClick.AddListener(ShowMatchMakingPanel);
        OpenGlobalPlayersButton.onClick.AddListener(ShowGlobalPlayersPanel);
        foreach(Button BackButton in BackButtons)
        {
            BackButton.onClick.AddListener(ShowHomePanel);
        }
    }

    void ShowHomePanel()
    {
        FriendsPanel.SetActive(false);
        HomePanel.SetActive(true);
        MatchMakingPanel.SetActive(false);
        GlobalPlayersPanel.SetActive(false);
    }

    void ShowFriendsPanel()
    {
        RefreshFriendList();
        FriendsPanel.SetActive(true);
        HomePanel.SetActive(false);
        MatchMakingPanel.SetActive(false);
        GlobalPlayersPanel.SetActive(false);
    }

    void ShowGlobalPlayersPanel()
    {
        FriendsPanel.SetActive(false);
        HomePanel.SetActive(false);
        MatchMakingPanel.SetActive(false);
        GlobalPlayersPanel.SetActive(true);
    }

    void ShowMatchMakingPanel()
    {
        FriendsPanel.SetActive(false);
        HomePanel.SetActive(false);
        MatchMakingPanel.SetActive(true);
        GlobalPlayersPanel.SetActive(false);
    }
    #endregion

    void RefreshFriendList()
    {
        // To do
        //FetchData

    }
    
}
