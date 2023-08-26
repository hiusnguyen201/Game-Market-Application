namespace GMA.Models;

public class Genre
{
    public int GenreId { get; set; }
    public string GenreName { get; set; }

    public Genre() { }
    public Genre(int GenreId, string GenreName) 
    { 
        this.GenreId = GenreId;
        this.GenreName = GenreName;
    }
}