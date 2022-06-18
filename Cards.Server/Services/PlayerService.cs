using Cards.Core.Game;

namespace Cards.Server.Services;
public class PlayerService : IPlayerService
{
    public IDictionary<string, Player> IdToPlayer { get; } = new Dictionary<string, Player>();
}
