using GMA.Models;
using GMA.DAL;
using GMA.BLL;

namespace GMA.Tests;

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


    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 2)]
    public void TestSaveDetails(int oid, int gid)
    {
        int expectedResult = orderBLL.SaveDetails(oid, gid);
        Assert.True(expectedResult != 0);
    }


    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    public void TestGetAll(int accid)
    {
        List<Order> orders = orderBLL.GetAll(accid);
        Assert.True(orders.Count != 0);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    public void GetById(int oid)
    {
        Order order = orderBLL.GetById(oid);
        Assert.True(order != null);
    }
}