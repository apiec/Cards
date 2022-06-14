using Cards.Core.Dto;
using Cards.Core.Game;
using Cards.Server.Services;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Immutable;

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

    public Task SubmitCards(WhiteCard[] cards)
    {
        return _gameService.SubmitCards(Context.ConnectionId, cards);
    }

    public Task PickWinner(string winningPlayerId)
    {
        return _gameService.PickWinner(winningPlayerId);
    }
}

