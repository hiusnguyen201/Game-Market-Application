using GMA.BLL;
using GMA.Models;

namespace GMA.Tests;

public class GenreBLLTest 
{
    private GenreBLL genreBLL = new GenreBLL();

    [Theory]
    [InlineData(1)]
    [InlineData(11)]
    [InlineData(0)]
    public void TestSearchById(int id)
    {
        Genre Genre = genreBLL.SearchById(id);
        Assert.True(Genre != null);
    }
}