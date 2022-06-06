using Cards.Server.Services;
using Microsoft.AspNetCore.SignalR;

namespace Cards.Server.SignalR;
public class GameHub : Hub<IGameClient>
{
    private readonly IGameService _gameService;

    public GameHub(IGameService gameService)
    {
        _gameService = gameService;
    }

    public override Task OnConnectedAsync()
    {
        _gameService.Game.Players.Create(Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
}

