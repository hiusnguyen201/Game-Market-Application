namespace GMA.Models;

public static class CartStatus
{
    public const int EMPTY = 1;
    public const int ACTIVE = 2;
    public const int INACTIVE = 3;
    public const int ABANDONED = 4;
}

public class Cart
{
    public int CartId { get; set; }
    public Account CartAccount { get; set; }
    public double TotalPrice { get; set; }
    public DateTime CreateDate { get; set; }
    public int Status { get; set; }
    public List<Game> CartItems { get; set; }

    public Game this[int index]
    {
        get
        {
            if (CartItems == null || CartItems.Count == 0 || index < 0 || CartItems.Count < index) return null;
            return CartItems[index];
        }
        set
        {
            if (CartItems == null) CartItems = new List<Game>();
            CartItems.Add(value);
        }
    }

    public Cart()
    {
        CartItems = new List<Game>();
    }

    public Cart(Account CartAccount, double TotalPrice)
    {
        this.CartAccount = CartAccount;
        this.TotalPrice = TotalPrice;
        this.Status = CartStatus.EMPTY;
        CartItems = new List<Game>();
    }

    public override bool Equals(object? obj)
    {
        if (obj is Cart) ((Cart)obj).CartId.Equals(CartId);
        return false;
    }

    public override int GetHashCode()
    {
        return CartId.GetHashCode();
    }
}