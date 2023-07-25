namespace GMA.Models;

public class Account
{
    public int id { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public string name { get; set; }
    public string phone { get; set; }
    public string email { get; set; }
    public string address { get; set; }
    public double money { get; set; }
    public DateTime createDate { get; set;}

    public Account () {}

    public Account (string username, string password, string name, string phone, string email, string address, double money, DateTime createDate)
    {
        this.username = username;
        this.password = password;
        this.name = name;
        this.phone = phone;
        this.email = email;
        this.address = address;
        this.money = money;
        this.createDate = createDate;
    }
}
