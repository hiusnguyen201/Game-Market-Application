namespace GMA.Models;

public static class OrderStatus
{
    public const int UNPAID = 0;
    public const int PAID = 1;
}

public class Order
{
    public int OrderId { get; set; }
    public Account OrderAccount { get; set; }
    public double TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Status { get; set; }
    public List<Game> OrderGames { get; set; }

    public Order()
    {
        this.OrderGames = new List<Game>();
    }

    public Order(Account OrderAccount, double TotalPrice)
    {
        this.OrderAccount = OrderAccount;
        this.TotalPrice = TotalPrice;
        this.Status = OrderStatus.UNPAID;
        this.OrderGames = new List<Game>();
    }
}