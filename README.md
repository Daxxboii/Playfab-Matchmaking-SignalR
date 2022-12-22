# Playfab-Matchmaking-SignalR
- A way for users to get realtime friend request notifications using SignalR and PlayFab as the backend services
- Allowing PlayFab users to create/join a party for matchmaking
- Allowing players to access and inspect all the online players globally within the title data in real time

> Note : The project only supports real time friend requests (Only when the target player is online) , to bypass this you can add a notification caching system that serves the notifications to the client on login 

## What does it do?
- Enables a player to accept/decine/send a friend requests to other players via SignalR
- Enables a player to send a party invite to a friend for matchmaking

> Note : The matchmaking part is handled by PlayFab

## Future Updates

- Polling notifications for offline users
- Adding UI Scene for matchmaking

## To-Do

- Add the CDN link in ```config.cs``` 
- Add Developer SecretKey and Title Id in ```PlayFabSharedSettings```
- Download and import Party SDK
> Link to the downloader [here](https://github.com/jpgordon00/UnityFileDownloader.git)
> Link to party SDK [here](https://github.com/PlayFab/PlayFabPartyUnity)
