namespace GMA.Models;

public class CartItem
{
    public int cartID { get; set; }
    public int gameID { get; set; }
    public int quantity { get; set;}

    public CartItem () {}

    public CartItem ( int cartID, int gameID, int quantity)
    {
        this.cartID = cartID;
        this.gameID = gameID;
        this.quantity = quantity;
    }
}