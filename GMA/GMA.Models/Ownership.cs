namespace GMA.Models;

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