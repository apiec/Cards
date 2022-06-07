namespace Cards.Core.Game;
public class PlayersManager
{
    readonly List<Player> _players = new();
    readonly LinkedList<Player> _playersQueue = new();

    LinkedListNode<Player>? _sequenceTail = null;

    public IReadOnlyList<Player> Players => _players.AsReadOnly();

    public Player Create(string name)
    {
        var newPlayer = new Player(name);
        _players.Add(newPlayer);
        if (_sequenceTail is not null)
        {
            var newNode = new LinkedListNode<Player>(newPlayer);
            _playersQueue.AddAfter(_sequenceTail, newNode);
            _sequenceTail = newNode;
        }
        else
        {
            _sequenceTail = new LinkedListNode<Player>(newPlayer);
            _playersQueue.AddFirst(_sequenceTail);
        }

        return newPlayer;
    }

    public void Remove(Player player)
    {
        if (_sequenceTail is null)
        {
            return;
        }

        if (player == _sequenceTail.Value)
        {
            _sequenceTail = _sequenceTail.Previous;
        }
        _players.Remove(player);
        _playersQueue.Remove(player);
    }

    internal Player GetNextCzar()
    {
        var czar = _playersQueue.First();
        _playersQueue.RemoveFirst();
        _playersQueue.AddLast(czar);
        return czar;
    }
}
