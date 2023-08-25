namespace GMA.Models;

public class Game
{
    public int GameId { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public double Price { get; set; }
    public int Rating { get; set; }
    public string Size { get; set; }
    public DateTime CreatedAt { get; set; }
    public Publisher GamePublisher { get; set; }
    public List<Genre> GameGenres { get; set; }

    public Game() 
    { 
        this.GamePublisher = new Publisher();
        this.GameGenres = new List<Genre>();
    }

    public Game(int Id) 
    {
        this.GameId = Id;
        this.GamePublisher = new Publisher();
        this.GameGenres = new List<Genre>();
    }
}