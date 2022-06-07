using Cards.Core.Game;

namespace Cards.Server.Services;

public class GameService : IGameService
{
    private readonly IConfiguration _configuration;
    private readonly ICardRepository _cardRepository;
    
    public GameService(IConfiguration configuration, ICardRepository cardRepository)
    {
        _configuration = configuration;
        _cardRepository = cardRepository;
        Game = CreateNewGame();
    }

    public Game Game { get; private set; }

    public void RestartGame() => Game = CreateNewGame();

    private Game CreateNewGame()
    {
        var section = _configuration.GetSection("GameOptions");
        var gameOptions = new GameOptions
        {
            HandSize = section.GetValue<int>("HandSize"),
            RoundLimit = section.GetValue<int>("RoundLimit")
        };
        return new Game(_cardRepository.BlackCards, _cardRepository.WhiteCards, gameOptions);
    }

}

