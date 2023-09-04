using GMA.BLL;
using GMA.Models;
using GMA.Utility;

namespace GMA.UnitTests;

public class AccountBLLTest
{
    private AccountBLL accountBLL = new AccountBLL();

    [Fact]
    public void TestSaveSuccessfully()
    {
        int expectedResult = accountBLL.Save(new Account("hieu123", EncryptionAES.Encrypt("123"), "nguyen hieu", "hieu123@gmail.com", "ha noi"));
        Assert.NotEqual(expectedResult, 0);
    }


    [Fact]
    public void TestSaveUnSuccessfully()
    {
        int expectedResult = 0;
        Account account = new Account();
        account = null;
        expectedResult = accountBLL.Save(account);
        Assert.Equal(expectedResult, 0);
    }


    [Theory]
    [InlineData(1, true)]
    [InlineData(-12, false)]
    [InlineData(1231, false)]
    public void TestSearchById(int id, bool expectedResult)
    {
        Account account = accountBLL.SearchById(id);
        bool isValid = (account != null) ? true : false;
        Assert.True(expectedResult == isValid);
    }


    [Theory]
    [InlineData("hieu123", true)]
    [InlineData("", false)]
    [InlineData("asdasdas", false)]
    public void TestSearchByUsername(string username, bool expectedResult)
    {
        Account account = accountBLL.SearchByUsername(username);
        bool isValid = (account != null) ? true : false;
        Assert.True(expectedResult == isValid);
    }


    [Theory]
    [InlineData("", false)]
    [InlineData("asdafas@gmail.com", false)]
    [InlineData("hieu123@gmail.com", true)]
    public void TestSearchByEmail(string email, bool expectedResult)
    {
        Account account = accountBLL.SearchByEmail(email);
        bool isValid = (account != null) ? true : false;
        Assert.True(expectedResult == isValid);
    }


    [Theory]
    [InlineData("hieu111", "", false)]
    [InlineData("", "asdasda", false)]
    [InlineData("hieu123", "123", true)]
    public void TestSearchAccountLogin(string username, string password, bool expectedResult)
    {
        password = EncryptionAES.Encrypt(password);
        Account account = accountBLL.SearchAccountLogin(username, password);
        bool isValid = (account != null) ? true : false;
        Assert.True(expectedResult == isValid);
    }


    [Theory]
    [InlineData(1, 50, true)]
    [InlineData(-1, 100, false)]
    [InlineData(44, 11, false)]
    public void TestUpdateMoney(int accId, double mn, bool expectedResult)
    {
        int result = accountBLL.UpdateMoney(accId, mn);
        bool isValid = (result != 0) ? true : false;
        Assert.True(expectedResult == isValid);
    }
}