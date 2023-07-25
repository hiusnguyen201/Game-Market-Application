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