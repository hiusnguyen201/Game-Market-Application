using GMA.BLL;
using GMA.Models;

namespace GMA.UnitTests;

public class GenreBLLTest 
{
    private GenreBLL genreBLL = new GenreBLL();

    [Theory]
    [InlineData(1, true)]
    [InlineData(123, false)]
    [InlineData(-123, false)]
    public void TestSearchById(int id, bool expectedResult)
    {
        Genre genre = genreBLL.SearchById(id);
        bool isValid = (genre != null)? true : false;
        Assert.True(expectedResult == isValid);
    }

    [Fact]
    public void TestGetAll()
    {
        List<Genre> genres = genreBLL.GetAll();
        bool isValid = (genres.Count != 0)? true : false;
        Assert.True(isValid == true);
    }
}