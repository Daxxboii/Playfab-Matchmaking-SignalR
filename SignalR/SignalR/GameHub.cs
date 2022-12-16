using Microsoft.AspNetCore.SignalR;

public class GameHub : Hub
{
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

}