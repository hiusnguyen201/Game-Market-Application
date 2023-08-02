using GMA.DAL;
using GMA.Models;

namespace GMA.BLL;

public class GenreBLL
{
    public List<Genre> GetAll()
    {
        GenreDAL genreDAL = new GenreDAL();
        List<Genre> genres = genreDAL.GetAll();
        return genres;
    }
}