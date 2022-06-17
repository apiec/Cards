using Cards.Core.Game;

namespace Cards.Server.Services;
public interface IGameProvider
{
    Game Game { get; }
    void RestartGame();
}
