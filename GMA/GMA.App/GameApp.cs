namespace GMA.App;
using GMA.BLL;
using Spectre.Console;
using GMA.Models;

public class GameApp
{
    public static void GameMenu()
    {   
        Console.Clear();
        var table = new Table();
        table.AddColumn(new TableColumn(new Text("Game Name").Centered()));
        table.AddColumn(new TableColumn(new Text("Release Date").Centered()));
        table.AddColumn(new TableColumn(new Text("Rating").Centered()));
        table.AddColumn(new TableColumn(new Text("Price").Centered()));
        GameBLL gameBLL = new GameBLL();
        List<Game> games = gameBLL.GetAll();
        foreach (Game game in games)
        {
            table.AddRow($"{game.Name}", $"{game.ReleaseDate}", $"{game.Rating}", $"{game.Price}");
        }
        AnsiConsole.Write(table);
    }
}