namespace Cards.Core.Game;
public class Player
{
    public Player(Guid guid, string name)
    {
        Guid = guid;
        Name = name;
    }

    public Guid Guid { get; }
    public string Name { get; }
    public int Score { get; set; } = 0;

    public HashSet<WhiteCard> WhiteCards { get; } = new();
}

