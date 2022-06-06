using System.Collections.Immutable;

namespace Cards.Game;

public readonly record struct GameState
{
    public ImmutableList<WhiteCard> UserCards { get; }
    public ImmutableList<WhiteCard> SelectedCards { get; } 
    public BlackCard BlackCard { get; }


}
