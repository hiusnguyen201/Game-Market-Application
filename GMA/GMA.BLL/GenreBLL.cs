using GMA.DAL;
using GMA.Models;

namespace GMA.BLL;

public class GenreBLL
{
    public List<Genre> SearchByKey(string key)
    {
        GenreDAL genreDAL = new GenreDAL();
        List<Genre> genres = genreDAL.GetByKey(key);
        return genres;
    }

    public Genre SearchById(int id)
    {
        GenreDAL genreDAL = new GenreDAL();
        Genre genre = genreDAL.GetById(id);
        return genre;
    }
}