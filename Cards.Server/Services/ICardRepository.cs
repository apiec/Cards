using Cards.Core.Game;

namespace Cards.Server.Services
{
    public interface ICardRepository
    {
        IEnumerable<WhiteCard> WhiteCards { get; }
        IEnumerable<BlackCard> BlackCards { get; }
    }
}