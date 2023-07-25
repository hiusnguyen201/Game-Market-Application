namespace GMA.Models;

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