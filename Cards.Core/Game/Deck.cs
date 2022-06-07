namespace Cards.Core.Game;

internal class Deck<T>
{
    Random _rng = new Random();
    List<T> _deck;
    private int _deckTop;

    public Deck(IEnumerable<T> values)
    {
        _deck = values.ToList();
        _deckTop = _deck.Count - 1;
        Shuffle();
    }

    public int CountLeft => _deckTop + 1;

    public T Deal()
    {
        try
        {
            return _deck[_deckTop--];
        }
        catch (ArgumentOutOfRangeException e)
        {
            throw new Exception("Deck is empty, reset it to use it again.", e);
        }
    }

    public void Reset()
    {
        Shuffle();
        _deckTop = _deck.Count - 1;
    }

    private void Shuffle()
    {
        int n = _deck.Count;
        while (n > 1)
        {
            n--;
            int k = _rng.Next(n + 1);
            (_deck[k], _deck[n]) = (_deck[n], _deck[k]);
        }
    }
}

