namespace GMA.Models;

public class Cart
{
    public int id { get; set; }
    public int accountID { get; set; }
    public double totalPrice { get; set; }
    public DateTime createDate { get; set; }

    public Cart () {}

    public Cart (int accountID, double totalPrice)
    {
        this.accountID = accountID;
        this.totalPrice = totalPrice;
    }
}

public class CartItem
{
    public int cartID { get; set; }
    public int gameID { get; set; }

    public CartItem () {}

    public CartItem ( int cartID, int gameID, int quantity)
    {
        this.cartID = cartID;
        this.gameID = gameID;
    }
}