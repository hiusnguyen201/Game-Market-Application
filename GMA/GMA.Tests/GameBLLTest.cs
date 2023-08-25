using GMA.BLL;
using GMA.Models;

namespace GMA.Tests;

public class GameBLLTest 
{
    private GameBLL gameBLL = new GameBLL();

    [Theory]
    [InlineData(1)]
    [InlineData(11)]
    [InlineData(0)]
    public void TestSearchById(int id)
    {
        Game game = gameBLL.SearchById(id);
        Assert.True(game != null);
    }

    [Theory]
    [InlineData("", -1)]
    [InlineData("", 1)]
    [InlineData("23232", 1)]
    public void TestSearchByGenIdKey(string kw, int gid)
    {
        List<Game> games = gameBLL.SearchByGenIdKey(kw, gid);
        Assert.True(games.Count != 0);
    }
}