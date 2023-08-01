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

    public List<Game> GetAll()
    {
        GameDAL gameDAL = new GameDAL();
        List<Game> games = gameDAL.GetAll();
        return games;
    }
}