using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;
using PlayFab;
using PlayFab.ClientModels;
public class GameplayMenuManager : MonoBehaviour
{

    public static List<string> OnlinePlayers;
    public static List<PlayerDataPanel> AllGlobalDataPanels = new List<PlayerDataPanel>();
    public static List<PlayerDataPanel> AllFriendRequests = new List<PlayerDataPanel>();
    public static List<string> AllFriends = new List<string>();



    [SerializeField,Foldout("Panels",true)]
    private GameObject HomePanel,FriendsPanel,FriendRequestsPanel,MatchMakingPanel,GlobalPlayersPanel;
    [SerializeField]private GameObject GlobalPlayerList, FriendList,FriendRequestList;
    
    [SerializeField, Foldout("Buttons", true)]
    private Button OpenFriendsPanelButton, OpenMatchMakingPanelButton,OpenGlobalPlayersButton,OpenFriendRequestsButton;
    [SerializeField]private Button[] BackButtons;

    [SerializeField, Foldout("Other", true)]
    private GameObject GlobalPlayerInfoPanelPrefab,FriendRequestInfoPanelPrefab;

    public static GameplayMenuManager instance;


    void Awake()
    {
        if (instance == null) instance = this;
        MapAllButtons();
        ShowHomePanel();
    }


    #region Mapping
    void MapAllButtons()
    {
        OpenFriendsPanelButton.onClick.AddListener(ShowFriendsPanel);
        OpenMatchMakingPanelButton.onClick.AddListener(ShowMatchMakingPanel);
        OpenGlobalPlayersButton.onClick.AddListener(ShowGlobalPlayersPanel);
        OpenFriendRequestsButton.onClick.AddListener(ShowFriendRequestsPanel);
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
        FriendRequestsPanel.SetActive(false);
    }

    void ShowFriendsPanel()
    {
       // RefreshFriendList();
       PlayFabManager.instance.GetFriendList();
        FriendsPanel.SetActive(true);
        HomePanel.SetActive(false);
        MatchMakingPanel.SetActive(false);
        GlobalPlayersPanel.SetActive(false);
        FriendRequestsPanel.SetActive(false);
    }

    void ShowGlobalPlayersPanel()
    {
        RefreshGlobalList();
        FriendsPanel.SetActive(false);
        HomePanel.SetActive(false);
        MatchMakingPanel.SetActive(false);
        GlobalPlayersPanel.SetActive(true);
        FriendRequestsPanel.SetActive(false);
    }

    void ShowMatchMakingPanel()
    {
        FriendsPanel.SetActive(false);
        HomePanel.SetActive(false);
        MatchMakingPanel.SetActive(true);
        GlobalPlayersPanel.SetActive(false);
        FriendRequestsPanel.SetActive(false);
    }

    void ShowFriendRequestsPanel(){
        FriendsPanel.SetActive(false);
        HomePanel.SetActive(false);
        MatchMakingPanel.SetActive(false);
        GlobalPlayersPanel.SetActive(false);
        FriendRequestsPanel.SetActive(true);

    }
    #endregion
#region GLobal List
    async void RefreshGlobalList()
    {
        SignalRConnection.instance.FetchAllOnlinePlayers();
    }

    public void WriteGlobalList()
    {
        //Cleanup
        AllGlobalDataPanels.Clear();
        foreach (Transform T in GlobalPlayerList.transform)
        {
            Destroy(T.gameObject);
        }

        //Spawning
        foreach (string Name in OnlinePlayers)
        {
            if (Name != PlayFabManager.PlayerUsername)
            {
                var listprefab = Instantiate(GlobalPlayerInfoPanelPrefab);
                listprefab.transform.SetParent(GlobalPlayerList.transform);

                PlayerDataPanel data = listprefab.GetComponent<PlayerDataPanel>();
                data.Username = Name;
                data.IsOnline = true;

                data.Type = PlayerDataPanel.PlayerType.GlobalPlayer;
                data.UpdatePanel();
                AllGlobalDataPanels.Add(data);
            }
        }
    }
    #endregion

    #region FriendReqs

    public void WriteFriendRequests(string Username)
    {
        Debug.Log("Writing Friend Reqs");
        var listprefab = Instantiate(FriendRequestInfoPanelPrefab);
        listprefab.transform.SetParent(FriendRequestList.transform);

        PlayerDataPanel data = listprefab.GetComponent<PlayerDataPanel>();
                data.Username = Username;
                data.IsOnline = false;
                data.Type = PlayerDataPanel.PlayerType.FriendRequest;
                data.UpdatePanel();
                AllFriendRequests.Add(data);
    }

    #endregion

    #region Friends

    public void WriteFriendList(List<FriendInfo> friendsCache){
        friendsCache.ForEach(f => {
            Debug.Log(f.FriendPlayFabId);
            var listprefab = Instantiate(GlobalPlayerInfoPanelPrefab);
            listprefab.transform.SetParent(FriendList.transform);

            PlayerDataPanel data = listprefab.GetComponent<PlayerDataPanel>();
                data.Username = f.FriendPlayFabId;
                data.IsOnline = false;
                data.Type = PlayerDataPanel.PlayerType.Friend;
                data.UpdatePanel();
                AllFriendRequests.Add(data);
            
            }); }
    }
    


    #endregion
    

