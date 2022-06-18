namespace Cards.Core.Game;
public abstract class GameState
{
    protected readonly Game _game;
    public GameState(Game game)
    {
        _game = game;
    }

    public void StartNewRound()
    {
        if (_game.RoundNumber >= _game.GameOptions.RoundLimit)
        {
            _game.State = new EndOfGameState(_game);
        }

        DealCards();

        var czar = _game.Players.GetNextCzar();
        var blackCard = _game.BlackCards.Deal();

        _game.CurrentRound = new ActiveRound(blackCard, czar, _game.Players.Players);
        _game.RoundNumber += 1;
        _game.State = new StartOfRoundState(_game);
    }

    private void DealCards()
    {
        foreach(var player in _game.Players.Players)
        {
            while(player.WhiteCards.Count < _game.GameOptions.HandSize)
            {
                player.WhiteCards.Add(_game.WhiteCards.Deal());
            }
        }
    }

}

public class StartOfGameState : GameState
{
    public StartOfGameState(Game game) : base(game)
    {
    }
}

public class StartOfRoundState : GameState
{
    public StartOfRoundState(Game game) : base(game)
    {
    }

    public void StartRound()
    {
        _game.State = new PlayersPickingState(_game);
    }
}

public class PlayersPickingState : GameState
{
    public PlayersPickingState(Game game) : base(game)
    {
    }

    public void SubmitPickedCards(Player player, int[] cardIds)
    {
        ValidateSubmission(player, cardIds);

        var whiteCards = player.WhiteCards
            .Where(c => cardIds.Contains(c.Id))
            .ToArray();

        _game.CurrentRound.PickedCards.Add(player, whiteCards);
        foreach(var card in whiteCards)
        {
            player.WhiteCards.Remove(card);
        }

        if (AllPlayersSubmitted())
        {
            _game.State = new CzarPickingState(_game);
        }
    }

    private void ValidateSubmission(Player player, int[] cardIds)
    {
        if (_game.CurrentRound.PickedCards.ContainsKey(player))
        {
            throw new Exception($"Player: {player} already submitted cards this round!");
        }

        if (!_game.CurrentRound.ActivePlayers.Contains(player))
        {
            throw new Exception($"Player: {player} is not an active player for this round!");
        }

        var blankCount = _game.CurrentRound.BlackCard.BlankCount;
        if (blankCount != cardIds.Length)
        {
            throw new Exception($"Wrong amount of submitted cards! " +
                $"Received {cardIds.Length} but need {blankCount}");
        }

        var cardsNotOwned = cardIds.Where(id => !player.WhiteCards.Any(c => c.Id == id));
        if (cardsNotOwned.Any())
        {
            throw new Exception($"Player: {player} does not own cards " +
                $"with ids: {string.Join(", ", cardsNotOwned)}");
        }
    }

    private bool AllPlayersSubmitted()
    {
        var round = _game.CurrentRound;
        return round.ActivePlayers.All(p =>
            round.PickedCards.ContainsKey(p));
    }
}

public class CzarPickingState : GameState
{
    public CzarPickingState(Game game) : base(game)
    {
    }

    public void PickWinner(Player player)
    {
        ValidateWinner(player);
        _game.CurrentRound.Winner = player;
        _game.State = new EndOfRoundState(_game);
    }

    private void ValidateWinner(Player player)
    {
        if (!_game.CurrentRound.PickedCards.ContainsKey(player))
        {
            throw new Exception($"Player: {player} did not submit their cards this round");
        }
    }
}

public class EndOfRoundState : GameState
{
    public EndOfRoundState(Game game) : base(game)
    {
    }

    public void EndRound()
    {
        if (_game.CurrentRound.Winner is not null)
        {
            _game.CurrentRound.Winner.Score += 1;
        }
        StartNewRound();
    }
}

public class EndOfGameState : GameState
{
    public EndOfGameState(Game game) : base(game)
    {
    }


}