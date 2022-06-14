using Cards.Core.Dto;
using Cards.Core.Game;
using Cards.Server.Services;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Immutable;

namespace Cards.Server.SignalR;
public class GameHub : Hub<IGameClient>
{
    private readonly IGameProvider _gameService;
    private readonly IPlayerService _playerService;

    public GameHub(IGameProvider gameService, IPlayerService playerService)
    {
        _gameService = gameService;
        _playerService = playerService;
    }

    public override Task OnConnectedAsync()
    {
        var player = _gameService.Game.Players
            .Create(Context.ConnectionId, Guid.NewGuid().ToString()); //guid as name for now

        _playerService.ConnectionToPlayer[Context.ConnectionId] = player;

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (_playerService.ConnectionToPlayer.TryGetValue(Context.ConnectionId, out var player))
        {
            _gameService.Game.Players.Remove(player);
            _playerService.ConnectionToPlayer.Remove(Context.ConnectionId);
        }
        return base.OnDisconnectedAsync(exception);
    }

    public Task StartNewRound()
    {
        _gameService.Game.State.StartNewRound();
        return UpdateGameStateForEveryone();
    }

    public Task SubmitCards(WhiteCard[] cards)
    {
        if (_gameService.Game.State is PlayersPickingState state)
        {
            var player = _playerService.ConnectionToPlayer[Context.ConnectionId];
            state.SubmitPickedCards(player, cards);
        }

        return UpdateGameStateForEveryone();
    }

    public Task PickWinner(string winningPlayerId)
    {
        if (_gameService.Game.State is CzarPickingState state)
        {
            var player = _playerService.ConnectionToPlayer[winningPlayerId];
            state.PickWinner(player);
        }

        return UpdateGameStateForEveryone();
    }

    private Task UpdateGameStateForEveryone()
    {

        var blackCard = _gameService.Game.RoundData.BlackCard;
        var czarId = _gameService.Game.RoundData.Czar.Id;

        var playerDtos = _gameService.Game.Players.Players
            .Select(p => new PlayerDto(p.Id, p.Name, p.Score))
            .ToImmutableList();

        var gameState = _gameService.Game.State switch
        {
            StartOfGameState => GameStateEnum.StartOfGame,
            StartOfRoundState => GameStateEnum.StartOfRound,
            PlayersPickingState => GameStateEnum.PlayersPicking,
            CzarPickingState => GameStateEnum.CzarPicking,
            EndOfRoundState => GameStateEnum.EndOfRound,
            EndOfGameState => GameStateEnum.EndOfGame,

            _ => throw new Exception($"Unknown game state: {nameof(_gameService.Game.State)}")
        };


        var gameSnapshot = new GameSnapshotDto(
            BlackCard: blackCard,
            CzarId: czarId,
            Players: playerDtos,
            GameState: gameState);

        var updateGameStateTasks = _playerService.ConnectionToPlayer.Select(kvp =>
        {
            var (id, player) = kvp;
            var playerSnapshot = new PlayerSpecificGameSnapshotDto(
                PlayerId: id, 
                GameSnapshot: gameSnapshot, 
                Hand: player.WhiteCards.ToImmutableList());

            return Clients.Client(id).UpdateGameState(playerSnapshot);
        });

        return Task.WhenAll(updateGameStateTasks);
    }
}

