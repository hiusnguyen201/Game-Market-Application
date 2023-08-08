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
    public DateTime OrderDate { get; set; }
    public int Status { get; set; }
    public List<Game> OrderDetails { get; set; }

    public Order(){}

    public Order(Account OrderAccount, double TotalPrice)
    {
        this.OrderAccount = OrderAccount;
        this.TotalPrice = TotalPrice;
        this.Status = OrderStatus.UNPAID;
        OrderDetails = new List<Game>();
    }
}