using GMA.DAL;
using GMA.Models;

namespace GMA.BLL;

public class GameBLL
{
    private GameDAL gameDAL = new GameDAL();
    public Game SearchById(int id) => gameDAL.GetById(id);

    public List<Game> SearchByKey(string keyword) => gameDAL.GetByKey(keyword);

    public List<Game> SearchByGenIdKey(string keyword, int id) => gameDAL.GetByGenIdKey(keyword, id);
}