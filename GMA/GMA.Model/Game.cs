
namespace GMA.Models;

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

    public Game(int Id) 
    {
        this.GameId = Id;
        GamePublisher = new Publisher();
        GameGenres = new List<Genre>();
    }
}