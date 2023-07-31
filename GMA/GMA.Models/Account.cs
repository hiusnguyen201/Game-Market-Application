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

    public Account() { }

    public Account(string Username, string Password, string Realname, string Email, string Address)
    {
        this.Username = Username;
        this.Password = Password;
        this.Realname = Realname;
        this.Email = Email;
        this.Address = Address;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Account) ((Account)obj).AccountId.Equals(AccountId);
        return false;
    }

    public override int GetHashCode()
    {
        return AccountId.GetHashCode();
    }
}