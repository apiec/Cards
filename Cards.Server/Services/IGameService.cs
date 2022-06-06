using Cards.Core;

namespace Cards.Server.Services
{
    public interface IGameService
    {
        Game Game { get; }
        void RestartGame();
    }
}