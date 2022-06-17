using Cards.Core.Game;

namespace Cards.Server.Services;
public class PlayerRepository : IPlayerService
{
    public IDictionary<string, Player> IdToPlayer { get; } = new Dictionary<string, Player>();
}
