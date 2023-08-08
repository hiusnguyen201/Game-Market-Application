using GMA.DAL;
using GMA.Models;

namespace GMA.BLL;

public class GameBLL
{
    public Game SearchById(int id)
    {
        GameDAL gameDAL = new GameDAL();
        Game game = gameDAL.GetById(id);
        return game;
    }

    public List<Game> SearchByKey(string keyword)
    {
        GameDAL gameDAL = new GameDAL();
        List<Game> games = gameDAL.GetByKey(keyword);
        return games;
    }

    public List<Game> SearchByGenIdKey(string keyword, int id)
    {
        GameDAL gameDAL = new GameDAL();
        List<Game> games = gameDAL.GetByGenIdKey(keyword, id);
        return games;
    }
}