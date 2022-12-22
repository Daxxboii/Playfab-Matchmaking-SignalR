using Microsoft.AspNetCore.SignalR;
public class GameHub : Hub
{

    const string OnlinePlayersGroupName ="OnlinePlayers";
    public static Dictionary<string,string> OnlinePlayers = new Dictionary<string,string>();


    public async Task RegisterPlayerOnline(string PlayerUserName){
        
         OnlinePlayers.Add(PlayerUserName,Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId,OnlinePlayersGroupName);

    }

    public async Task DeRegisterPlayerOnline(string PlayerUserName){
        await Groups.RemoveFromGroupAsync(Context.ConnectionId,OnlinePlayersGroupName);
        OnlinePlayers.Remove(PlayerUserName);
    }

    public async Task SendAllOnlinePlayers(string PlayerUserName)
    {
         await Clients.Client(OnlinePlayers[PlayerUserName]).SendAsync("ListOfOnlinePlayers",OnlinePlayers);
    }
    public async Task SendDataToGroup(string data, string GroupName)
    {
        await Clients.Group(GroupName).SendAsync("GamePlayData", data);
    }

    //Called From Clients
    public async Task AddToQueue(string ChannelName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, ChannelName);
    }

    public async Task RemoveFromQueue(string ChannelName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, ChannelName);
    }

    public async Task RelayFriendRequestData(string PlayerUsername,string TargetUsername,string RequestStatus){
       
        Dictionary<string,string> FriendRequestData = new Dictionary<string, string>();
        FriendRequestData.Add(PlayerUsername,RequestStatus);
        await Clients.Client(OnlinePlayers[TargetUsername]).SendAsync("FriendRequestData",FriendRequestData);
    }

    public async Task OnPartyInvite(string PlayerUsername,string TargetUsername,string NetworkId){
         Dictionary<string,string> PartyInviteData = new Dictionary<string, string>();
        PartyInviteData.Add(PlayerUsername,NetworkId);
        await Clients.Client(OnlinePlayers[TargetUsername]).SendAsync("PartyInvite",PartyInviteData);
    }

    public async Task SendTicketID(string TargetUsername,string TicketID){
        await Clients.Client(OnlinePlayers[TargetUsername]).SendAsync("TicketID",TicketID);

    }
}