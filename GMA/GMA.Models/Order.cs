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