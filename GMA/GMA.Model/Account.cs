namespace GMA.Models;

public class Account
{
    public int AccountId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Realname { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public double Money { get; set; }
    public DateTime CreateDate { get; set; }
    public List<Order> AccountOrders { get; set; }

    public Account() 
    {
        AccountOrders = new List<Order>();
    }

    public Account(int Id) 
    {
        this.AccountId = Id;
        this.AccountOrders = new List<Order>();
    }

    public Account(string Username, string Password, string Realname, string Email, string Address)
    {
        this.Username = Username;
        this.Password = Password;
        this.Realname = Realname;
        this.Email = Email;
        this.Address = Address;
        AccountOrders = new List<Order>();
    }
}