using GMA.DAL;
using GMA.Models;

namespace GMA.BLL;

public class GenreBLL
{
    private GenreDAL genreDAL = new GenreDAL();
    public List<Genre> SearchByKey(string key) => genreDAL.GetByKey(key);
    public Genre SearchById(int id) => genreDAL.GetById(id);
}