using Cards.Core.Game;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Cards.Core.Dto;
public record GameSnapshot(
    BlackCard BlackCard,
    Player Czar,
    ImmutableList<Player> Players,
    GameStateEnum GameState
);
