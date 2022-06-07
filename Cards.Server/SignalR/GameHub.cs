using Cards.Server.Services;
using Microsoft.AspNetCore.SignalR;

namespace Cards.Server.SignalR;
public class GameHub : Hub<IGameClient>
{
    private readonly IGameService _gameService;
    private readonly IPlayerService _playerService;

    public GameHub(IGameService gameService, IPlayerService playerService)
    {
        _gameService = gameService;
        _playerService = playerService;
    }

    public override Task OnConnectedAsync()
    {
        var player = _gameService.Game.Players.Create(Context.ConnectionId);
        _playerService.ConnectionToPlayer[Context.ConnectionId] = player;

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if(_playerService.ConnectionToPlayer.TryGetValue(Context.ConnectionId, out var player)) {
            _gameService.Game.Players.Remove(player);
            _playerService.ConnectionToPlayer.Remove(Context.ConnectionId);
        }
        return base.OnDisconnectedAsync(exception);
    }



}

