using GMA.Models;
using GMA.BLL;

namespace GMA.UnitTests;

public class OrderBLLTest
{
    private OrderBLL orderBLL = new OrderBLL();
    private AccountBLL accountBLL = new AccountBLL();

    [Fact]
    public void TestSaveSuccessfully()
    {
        Account account = accountBLL.SearchById(1);
        int expectedResult = orderBLL.Save(new Order(account, 23000));
        Assert.NotEqual(expectedResult, 0);
    }

    [Fact]
    public void TestSaveUnSuccessfully()
    {
        int expectedResult = 0;
        Order order = new Order();
        order = null;
        expectedResult = orderBLL.Save(order);
        Assert.Equal(expectedResult, 0);
    }


    [Fact]
    public void TestSaveDetailsSuccessfully()
    {
        int result = orderBLL.SaveDetails(1, 2);
        Assert.NotEqual(result, 0);
    }

    [Fact]
    public void TestSaveDetailsUnSuccessfully()
    {
        int result = orderBLL.SaveDetails(-2, -11);
        Assert.Equal(result, 0);
    }


    [Theory]
    [InlineData(1, true)]
    [InlineData(-123, false)]
    [InlineData(1000, false)]
    public void TestGetAll(int accid, bool expectedResult)
    {
        List<Order> orders = orderBLL.GetAll(accid);
        bool isValid = (orders.Count != 0)? true : false;
        Assert.True(expectedResult == isValid);
    }


    [Theory]
    [InlineData(1, true)]
    [InlineData(1000, false)]
    [InlineData(-123, false)]
    public void GetById(int oid, bool expectedResult)
    {
        Order order = orderBLL.GetById(oid);
        bool isValid = (order != null)? true : false;
        Assert.True(expectedResult == isValid);
    }
}