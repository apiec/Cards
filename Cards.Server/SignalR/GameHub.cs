using Cards.Core.Game;
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
        return Task.WhenAll(
            _gameService.CreatePlayer(Context.ConnectionId, Guid.NewGuid().ToString()),
            base.OnConnectedAsync());
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return Task.WhenAll(
            _gameService.RemovePlayer(Context.ConnectionId),
            base.OnDisconnectedAsync(exception));
    }

    public Task StartNewRound()
    {
        return _gameService.StartNewRound();
    }

    public Task SubmitCards(int[] cardIds)
    {
        return _gameService.SubmitCards(Context.ConnectionId, cardIds);
    }

    public Task PickWinner(string winningPlayerId)
    {
        return _gameService.PickWinner(winningPlayerId);
    }
}

