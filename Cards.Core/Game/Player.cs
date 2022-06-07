namespace Cards.Core.Game;
public class Player
{
    public Player(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public string Id { get; }
    public string Name { get; }
    public int Score { get; set; } = 0;

    public HashSet<WhiteCard> WhiteCards { get; } = new();
}

