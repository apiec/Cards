using Cards.Core.Dto;

namespace Cards.Server.SignalR
{
    public interface IGameClient
    {
        Task UpdateGameState(PlayerSpecificGameSnapshotDto snapshot);
    }
}
