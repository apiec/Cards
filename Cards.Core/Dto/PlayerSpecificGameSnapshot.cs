using Cards.Core.Game;
using System.Collections.Immutable;

namespace Cards.Core.Dto;
public record PlayerSpecificGameSnapshotDto(
    string PlayerId,
    GameSnapshotDto GameSnapshot,
    ImmutableList<WhiteCard> Hand
);

