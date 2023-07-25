namespace GMA.Models;

public class OrderDetail
{
    public int orderID { get; set; }
    public int gameID { get; set; }
    public int quantity { get; set; }

    public OrderDetail () {}

    public OrderDetail ( int orderID, int gameID, int quantity)
    {
        this.orderID = orderID;
        this.gameID = gameID;
        this.quantity = quantity;
    }
}