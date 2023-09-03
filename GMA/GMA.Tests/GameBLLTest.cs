using GMA.BLL;
using GMA.Models;

namespace GMA.UnitTests;

public class GameBLLTest 
{
    private GameBLL gameBLL = new GameBLL();

    [Theory]
    [InlineData(1, true)]
    [InlineData(-123, false)]
    [InlineData(1111, false)]
    public void TestSearchById(int id, bool expectedResult)
    {
        Game game = gameBLL.SearchById(id);
        bool isValid = (game != null)? true : false;
        Assert.True(expectedResult == isValid);
    }

    [Theory]
    [InlineData("", -1, false)]
    [InlineData("", 1, true)]
    [InlineData("23232", 1, false)]
    public void TestSearchByGenIdKey(string kw, int gid, bool expectedResult)
    {
        List<Game> games = gameBLL.SearchByGenIdKey(kw, gid);
        bool isValid = (games.Count != 0)? true : false;
        Assert.True(expectedResult == isValid);
    }
}