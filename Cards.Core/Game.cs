using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Core;

public class Game
{   
    public Game(IEnumerable<BlackCard> blackCards,
        IEnumerable<WhiteCard> whiteCards,
        GameOptions gameOptions)
    {
        BlackCards = new Deck<BlackCard>(blackCards);
        WhiteCards = new Deck<WhiteCard>(whiteCards);
        State = new StartOfGameState(this);
        Players = new PlayersManager();
        GameOptions = gameOptions;
    }

    public GameOptions GameOptions { get; }
    public PlayersManager Players { get; }
    public GameState State { get; internal set; }
    public int RoundNumber { get; internal set; }

    internal ActiveRound CurrentRound { get; set; } = null!;
    internal Deck<BlackCard> BlackCards { get; }
    internal Deck<WhiteCard> WhiteCards { get; }
}

public readonly struct GameOptions
{
    public int HandSize { get; }
    public int RoundLimit { get; }
}
