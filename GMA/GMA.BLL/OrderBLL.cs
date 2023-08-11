using GMA.DAL;
using GMA.Models;
using System.Text;

namespace GMA.BLL;

public class OrderBLL
{
    private OrderDAL orderDAL = new OrderDAL();
    public int Save( Order order)
    {
        int result = 0;
        if (order != null)
        {
            result = orderDAL.CreateOrder(order);
        }
        return result;
    }

    public int SaveDetails (int orderID, int gameID)
    {
        int result = 0;
        if(orderID != null && gameID != null)
        {
            result = orderDAL.CreateOrderDetails(orderID, gameID);
        }
        return result;
    }
    
    public List<Order> GetAll(int accid) => orderDAL.GetAll(accid);

    public Order GetById (int oid) => orderDAL.GetById(oid);
}