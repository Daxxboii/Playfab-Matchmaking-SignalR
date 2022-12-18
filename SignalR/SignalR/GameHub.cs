using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
public class GameHub : Hub
{

    const string OnlinePlayersGroupName ="OnlinePlayers";
    public Dictionary<string,string> OnlinePlayers = new Dictionary<string,string>();


    public async Task RegisterPlayerOnline(string PlayerUserName){
        
         OnlinePlayers.Add(PlayerUserName,Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId,OnlinePlayersGroupName);

    }

    public async Task DeRegisterPlayerOnline(string PlayerUserName){
        await Groups.RemoveFromGroupAsync(Context.ConnectionId,OnlinePlayersGroupName);
        OnlinePlayers.Remove(PlayerUserName);
    }

    public async Task SendAllOnlinePlayers(string UserName)
    {
        //string data = JsonConvert.SerializeObject(OnlinePlayers);
        await Clients.All.SendAsync("ListOfOnlinePlayers",OnlinePlayers );  
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

    public async Task SendFriendRequest(string Name){
        //To Do
    }

    public async Task SendBackConfirmation(string Name){
        //To Do
    }

}