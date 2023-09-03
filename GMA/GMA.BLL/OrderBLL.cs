using System.Dynamic;
using GMA.DAL;
using GMA.Models;

namespace GMA.BLL;

public class OrderBLL
{
    private OrderDAL orderDAL = new OrderDAL();
    private static List<Game> cartGames = new List<Game>();

    public double CalculateTotalPrice()
    {
        int count = cartGames.Count;
        if (count != 0)
        {
            double total = 0;
            foreach (Game game in cartGames)
            {
                total += game.Price;
            }
            return total;
        }
        return count;
    }

    public void AddGameToCart(Game game)
    {
        cartGames.Add(game);
    }

    public void RemoveGameFromCart(Game game)
    {
        cartGames.Remove(game);
    }

    public List<Game> GetCartGames()
    {
        return cartGames;
    }

    public void SetCartGames(List<Game> cart)
    {
        cartGames = cart;
    }

    public int Save(Order order)
    {
        int result = 0;
        if (order != null)
        {
            result = orderDAL.CreateOrder(order);
        }
        return result;
    }

    public int SaveDetails(int orderID, int gameID)
    {
        int result = 0;
        if (orderID != null && gameID != null)
        {
            result = orderDAL.CreateOrderDetails(orderID, gameID);
        }
        return result;
    }

    public List<Order> GetAll(int accid) => orderDAL.GetAll(accid);

    public Order GetById(int oid) => orderDAL.GetById(oid);
}