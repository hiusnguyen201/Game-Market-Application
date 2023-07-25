namespace GMA.Models;

public class Order
{
    public int id { get; set; }
    public int accountID { get; set; }
    public double totalPrice { get; set; }
    public DateTime orderDate { get; set; }
    public string orderStatus { get; set; }
    
    public Order () {}

    public Order (int accountID, double totalPrice, DateTime orderDate, string orderStatus)
    {
        this.accountID = accountID;
        this.totalPrice = totalPrice;
        this.orderDate = orderDate;
        this.orderStatus = orderStatus;
    }
}

public class OrderDetail
{
    public int orderID { get; set; }
    public int gameID { get; set; }
    public OrderDetail () {}

    public OrderDetail ( int orderID, int gameID, int quantity)
    {
        this.orderID = orderID;
        this.gameID = gameID;
    }
}