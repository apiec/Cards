using Cards.Core.Game;

namespace Cards.Server.Services
{
    public interface IPlayerService
    {
        public IDictionary<string, Player> ConnectionToPlayer { get; }
    }
}