namespace GMA.Models;

public class Publisher
{
    public int id { get; set; }
    public string publisherName { get; set; }

    public Publisher () {}

    public Publisher ( string publisherName)

    {
        this.publisherName = publisherName;
    }
}