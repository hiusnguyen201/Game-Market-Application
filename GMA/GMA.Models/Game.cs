namespace GMA.Models;

public class Game
{
    public int id { get; set; }
    public int publisherID { get; set; }
    public string Name { get; set; }
    public string description { get; set;}
    public double price { get; set; }
    public double rating { get; set; }
    public string size { get; set; }
    public string status { get; set; }
    public float discount { get; set; }
    public char discountUnit { get; set; }
    public DateTime releaseDate { get; set; }

    public Game () {}

    public Game ( int publisherID, string name, string description, double price, double rating, string size, string status, float discount, char discountUnit, DateTime releaseDate)

    { 
        this.publisherID = publisherID;
        this.Name = name;
        this.description = description;
        this.price = price;
        this.rating = rating;
        this.size = size;
        this.status = status;
        this.discount = discount;
        this.discountUnit = discountUnit;
        this.releaseDate = releaseDate;
    }
}