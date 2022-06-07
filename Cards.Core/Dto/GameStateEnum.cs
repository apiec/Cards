using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Core.Dto;
public enum GameStateEnum
{
    StartOfGame,
    StartOfRound,
    PlayersPicking,
    CzarPicking,
    EndOfRound,
    EndOfGame
}

