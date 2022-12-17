using Microsoft.AspNetCore.SignalR;

public class GameHub : Hub
{

    const string OnlinePlayersGroupName ="OnlinePlayers";
    public Dictionary<string,string> OnlinePlayers = new Dictionary<string,string>();


    public async Task RegisterPlayerOnline(string PlayerUserName){
        await Groups.AddToGroupAsync(Context.ConnectionId,OnlinePlayersGroupName);
        OnlinePlayers.Add(PlayerUserName,Context.ConnectionId);
    }

    public async async DeRegisterPlayerOnline(string PlayerUserName){
        await Groups.RemoveFromGroupAsync(Context.ConnectionId,OnlinePlayersGroupName);
        OnlinePlayers.Remove(PlayerUserName);
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