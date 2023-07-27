namespace GMA.Models;
public class Account
{
    public int AccountId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Realname { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public double Money { get; set; }
    public DateTime CreateDate { get; set; }
    public List<Game> OwnershipGames { set; get; }

    public Game this[int index]
    {
        get
        {
            if (OwnershipGames == null || OwnershipGames.Count == 0 || index < 0 || OwnershipGames.Count < index) return null;
            return OwnershipGames[index];
        }
        set
        {
            if (OwnershipGames == null) OwnershipGames = new List<Game>();
            OwnershipGames.Add(value);
        }
    }

    public Account()
    {
        OwnershipGames = new List<Game>();
    }

    public Account(string Username, string Password, string Realname, string Phone, string Email, string Address)
    {
        this.Username = Username;
        this.Password = Password;
        this.Realname = Realname;
        this.Phone = Phone;
        this.Email = Email;
        this.Address = Address;
        OwnershipGames = new List<Game>();
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