using Cards.Core.Game;
using System.Collections.Immutable;

namespace Cards.Core.Dto;
public record GameSnapshotDto(
    BlackCard BlackCard,
    string CzarId,
    ImmutableList<PlayerDto> Players,
    GameStateEnum GameState
);
