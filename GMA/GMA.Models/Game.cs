namespace GMA.Models;

public class Game
{
    public int id { get; set; }
    public int publisherID { get; set; }
    public string name { get; set; }
    public string desc { get; set;}
    public double price { get; set; }
    public float rating { get; set; }
    public string size { get; set; }
    public string status { get; set; }
    public float discount { get; set; }
    public char discountUnit { get; set; }
    public DateTime releaseDate { get; set; }

    public Game () {}

    public Game (int publisherID, string name, string description, double price, float rating, string size, string status, float discount)

    { 
        this.publisherID = publisherID;
        this.name = name;
        this.desc = desc;
        this.price = price;
        this.rating = rating;
        this.size = size;
        this.status = status;
        this.discount = discount;
    }
}