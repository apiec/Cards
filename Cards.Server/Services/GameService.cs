using Cards.Core.Dto;
using Cards.Core.Game;
using Cards.Server.SignalR;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Immutable;

namespace Cards.Server.Services;

public class GameService : IGameService
{
    private readonly IGameProvider _gameProvider;
    private readonly IPlayerService _playerService;
    private readonly IHubContext<GameHub, IGameClient> _hubContext;

    public GameService(IGameProvider gameProvider,
        IPlayerService playerService,
        IHubContext<GameHub, IGameClient> hubContext)
    {
        _gameProvider = gameProvider;
        _playerService = playerService;
        _hubContext = hubContext;
    }

    public Task CreatePlayer(string connectionId, string name)
    {
        var player = _gameProvider.Game.Players.Create(connectionId, name);
        _playerService.IdToPlayer[connectionId] = player;

        return UpdateGameStateForEveryone();
    }

    public Task RemovePlayer(string connectionId)
    {
        if (_playerService.IdToPlayer.TryGetValue(connectionId, out var player))
        {
            _gameProvider.Game.Players.Remove(player);
            _playerService.IdToPlayer.Remove(connectionId);
        }

        return UpdateGameStateForEveryone();
    }

    public Task StartNewRound()
    {
        _gameProvider.Game.State.StartNewRound();
        return UpdateGameStateForEveryone();
    }

    public Task SubmitCards(string connectionId, int[] cardIds)
    {
        if (_gameProvider.Game.State is PlayersPickingState state)
        {
            var player = _playerService.IdToPlayer[connectionId];
            state.SubmitPickedCards(player, cardIds);
        }
        return UpdateGameStateForEveryone();
    }

    public Task PickWinner(string winningPlayerId)
    {
        if (_gameProvider.Game.State is CzarPickingState state)
        {
            var player = _playerService.IdToPlayer[winningPlayerId];
            state.PickWinner(player);
        }
        return UpdateGameStateForEveryone();
    }

    private Task UpdateGameStateForEveryone()
    {
        var blackCard = _gameProvider.Game.RoundData.BlackCard;
        var czarId = _gameProvider.Game.RoundData.Czar.Id;

        var playerDtos = _gameProvider.Game.Players.Players
            .Select(p => new PlayerDto(p.Id, p.Name, p.Score))
            .ToImmutableList();

        var gameState = _gameProvider.Game.State switch
        {
            StartOfGameState => GameStateEnum.StartOfGame,
            StartOfRoundState => GameStateEnum.StartOfRound,
            PlayersPickingState => GameStateEnum.PlayersPicking,
            CzarPickingState => GameStateEnum.CzarPicking,
            EndOfRoundState => GameStateEnum.EndOfRound,
            EndOfGameState => GameStateEnum.EndOfGame,

            _ => throw new Exception($"Unknown game state: {nameof(_gameProvider.Game.State)}")
        };


        var gameSnapshot = new GameSnapshotDto(
            BlackCard: blackCard,
            CzarId: czarId,
            Players: playerDtos,
            GameState: gameState);

        var updateGameStateTasks = _playerService.IdToPlayer.Select(kvp =>
        {
            var (id, player) = kvp;
            var playerSnapshot = new PlayerSpecificGameSnapshotDto(
                PlayerId: id,
                GameSnapshot: gameSnapshot,
                Hand: player.WhiteCards.ToImmutableList());

            return _hubContext.Clients
                .Client(id)
                .UpdateGameState(playerSnapshot);
        });

        return Task.WhenAll(updateGameStateTasks);
    }
}
