namespace GMA.Models;

public class Account
{
    public int id { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public string realname { get; set; }
    public string phone { get; set; }
    public string email { get; set; }
    public string address { get; set; }
    public double money { get; set; }
    public DateTime createDate { get; set;}

    public Account () {}

    public Account (string username, string password, string realname, string phone, string email, string address)
    {
        this.username = username;
        this.password = password;
        this.realname = realname;
        this.phone = phone;
        this.email = email;
        this.address = address;
    }
}

public class Ownership
{
    public int accountID { get; set; }
    public int gameID { get; set; }

    public Ownership () {}

    public Ownership ( int accountID, int gameID)
    {
        this.accountID = accountID;
        this.gameID = gameID;
    }
}