namespace GMA.Models;

public class Cart
{
    public int id { get; set; }
    public int accountID { get; set; }
    public double totalPrice { get; set; }
    public DateTime createDate { get; set; }

    public Cart () {}

    public Cart (int accountID, double totalPrice, DateTime createDate)
    {
        this.accountID = accountID;
        this.totalPrice = totalPrice;
        this.createDate = createDate;
    }
}