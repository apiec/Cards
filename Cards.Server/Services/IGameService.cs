using Cards.Core.Game;

namespace Cards.Server.Services;
public interface IGameService
{
    Task CreatePlayer(string connectionId, string name);
    Task PickWinner(string winningPlayerId);
    Task RemovePlayer(string connectionId);
    Task StartNewRound();
    Task SubmitCards(string connectionId, WhiteCard[] cards);
}
