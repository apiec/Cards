namespace Cards.Core.Game;
public class Player
{
    public Player(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public int Score { get; set; } = 0;

    public HashSet<WhiteCard> WhiteCards { get; } = new();
}

