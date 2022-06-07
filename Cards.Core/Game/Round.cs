using System.Collections.Immutable;

namespace Cards.Core.Game;
public class ActiveRound
{
    public ActiveRound(BlackCard blackCard, Player czar, IEnumerable<Player> activePlayers)
    {
        BlackCard = blackCard;
        Czar = czar;
        ActivePlayers = activePlayers.ToHashSet();
        ActivePlayers.Remove(czar);
    }

    public BlackCard BlackCard { get; init; }
    public Player Czar { get; init; }
    public HashSet<Player> ActivePlayers { get; init; }

    public Player? Winner { get; set; }
    public Dictionary<Player, WhiteCard[]> PickedCards { get; } = new();
}


public readonly struct FinishedRound
{
    public FinishedRound(ActiveRound round)
    {
        BlackCard = round.BlackCard;
        Czar = round.Czar;
        ActivePlayers = round.ActivePlayers.ToImmutableHashSet();
        Winner = round.Winner;
        PickedCards = round.PickedCards.ToImmutableDictionary();

        RoundCompleted = round.Winner is null;
    }

    public BlackCard BlackCard { get; }
    public Player Czar { get; }
    public ImmutableHashSet<Player> ActivePlayers { get; }
    public Player? Winner { get; }
    public ImmutableDictionary<Player, WhiteCard[]> PickedCards { get; }
    public bool RoundCompleted { get; }
}
