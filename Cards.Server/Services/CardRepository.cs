using Cards.Core.Game;

namespace Cards.Server.Services;
public class CardRepository : ICardRepository
{
    public CardRepository()
    {
        //mock data
        var whiteCards = new List<WhiteCard>();
        for (int i = 0; i < 40; i++)
        {
            whiteCards.Add(new WhiteCard(i, $"card #{i}"));
        }
        WhiteCards = whiteCards;

        var blackCards = new List<BlackCard>();
        for (int i = 0; i < 10; i++)
        {
            blackCards.Add(new BlackCard(i, $"card #{i}", 1));
        }
        BlackCards = blackCards;
    }

    public IEnumerable<WhiteCard> WhiteCards { get; }

    public IEnumerable<BlackCard> BlackCards { get; }
}

