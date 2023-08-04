namespace GMA.Models;

public static class OrderStatus
{
    public const int PENDING = 1;
    public const int PROCESSING = 2;
    public const int COMPLETED = 3;
    public const int DECLINED = 4;
}

public class Order
{
    public int OrderId { get; set; }
    public Account OrderAccount { get; set; }
    public double TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
    public int Status { get; set; }
    public List<Game> OrderDetails { get; set; }

    public Game this[int index]
    {
        get
        {
            if (OrderDetails == null || OrderDetails.Count == 0 || index < 0 || OrderDetails.Count < index) return null;
            return OrderDetails[index];
        }
        set
        {
            if (OrderDetails == null) OrderDetails = new List<Game>();
            OrderDetails.Add(value);
        }
    }

    public Order()
    {
        OrderDetails = new List<Game>();
    }

    public Order(Account OrderAccount, double TotalPrice)
    {
        this.OrderAccount = OrderAccount;
        this.TotalPrice = TotalPrice;
        this.Status = OrderStatus.PENDING;
        OrderDetails = new List<Game>();
    }
}