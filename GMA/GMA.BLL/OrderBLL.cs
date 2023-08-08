using GMA.DAL;
using GMA.Models;
using System.Text;

namespace GMA.BLL;

public class OrderBLL
{
    public int Save( Order order)
    {
        int result = 0;
        if (order != null)
        {
            OrderDAL orderDAL = new OrderDAL();
            result = orderDAL.CreateOrder(order);
        }
        return result;
    }

    public int SaveDetails (int orderID, int gameID, double unitPrice)
    {
        OrderDAL orderDAL = new OrderDAL();
        return orderDAL.CreateOrderDetails(orderID, gameID, unitPrice);
    }
    
}