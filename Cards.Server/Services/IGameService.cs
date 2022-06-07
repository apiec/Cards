using Cards.Core.Game;

namespace Cards.Server.Services
{
    public interface IGameService
    {
        Game Game { get; }
        void RestartGame();
    }
}