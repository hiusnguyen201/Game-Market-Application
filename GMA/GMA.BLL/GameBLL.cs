using GMA.DAL;
using GMA.Models;

namespace GMA.BLL;

public class GameBLL
{
    private GameDAL gameDAL = new GameDAL();
    public Game SearchById(int id) => gameDAL.GetById(id);
    public List<Game> SearchByGenIdKey(string kw, int gid) => gameDAL.GetByGenIdKey(kw, gid);
}