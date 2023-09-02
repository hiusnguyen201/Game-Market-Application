using GMA.DAL;
using GMA.Models;

namespace GMA.BLL;

public class GenreBLL
{
    private GenreDAL genreDAL = new GenreDAL();
    public List<Genre> GetAll() => genreDAL.GetAll();
    public Genre SearchById(int id) => genreDAL.GetById(id);
}