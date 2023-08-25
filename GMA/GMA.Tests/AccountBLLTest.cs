using GMA.BLL;
using GMA.Models;
using GMA.Utility;

namespace GMA.Tests;

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
    [InlineData("hieu111")]
    [InlineData("")]
    [InlineData("asdasdas")]
    public void TestSearchByUsername(string username)
    {
        Account account = accountBLL.SearchByUsername(username);
        Assert.True(account != null);
    }


    [Theory]
    [InlineData("")]
    [InlineData("asdafas@gmail.com")]
    [InlineData("hiusnguyen201@gmail.com")]
    public void TestSearchByEmail(string email)
    {
        Account account = accountBLL.SearchByEmail(email);
        Assert.True(account != null);
    }


    [Theory]
    [InlineData("hieu111", "")]
    [InlineData("hieu201", "asdasda")]
    [InlineData("hieu123", "123")]
    public void TestSearchAccountLogin(string username, string password)
    {
        password = EncryptionAES.Encrypt(password);
        Account account = accountBLL.SearchAccountLogin(username, password);
        Assert.True(account != null);
    }


    [Theory]
    [InlineData(1, 50)]
    public void TestUpdateMoney (int accId, double mn)
    {
        int expectedResult = accountBLL.UpdateMoney(accId, mn);
        Assert.True(expectedResult != 0);
    }
}