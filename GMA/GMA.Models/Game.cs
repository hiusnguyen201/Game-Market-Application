
namespace GMA.Models;

public static class GameStatus
{
    public const int AVAILABLE = 1;
    public const int COMING_SOON = 2;

}

public class Game
{
    public int GameId { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public double Price { get; set; }
    public float? Rating { get; set; }
    public string Size { get; set; }
    public DateTime ReleaseDate { get; set; }
    public Publisher GamePublisher { get; set; }
    public List<Genre> GameGenres { get; set; }

    public Game() 
    { 
        GamePublisher = new Publisher();
        GameGenres = new List<Genre>();
    }
}