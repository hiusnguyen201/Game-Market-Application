namespace GMA.Models;

public class Genre
{
    public int id { get; set; }
    public string genreName { get; set; }

    public Genre () {}

    public Genre ( string genreName)
    {
        this.genreName = genreName;
    }
}

public class GenreDetail
{
    public int genreID { get; set; }
    public int gameID { get; set; }

    public GenreDetail () {}

    public GenreDetail ( int genreID, int gameID)
    {
        this.genreID = genreID;
        this.gameID = gameID;
    }
}