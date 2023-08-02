namespace GMA.App;
using GMA.BLL;
using Spectre.Console;
using GMA.Models;

public class GameApp
{
    public static void GameMenu(int currentPage = 0)
    {
        int pageSize = 10;
        GameBLL gameBLL = new GameBLL();
        List<Game> games = gameBLL.GetAll();

        while (true)
        {
            int startIndex = currentPage * pageSize;
            int endIndex = Math.Min(startIndex + pageSize, games.Count);

            Console.Clear();
            var table = new Table()
            .AddColumn(new TableColumn(new Text("ID").Centered()))
            .AddColumn(new TableColumn(new Text("Game Name").Centered()))
            .AddColumn(new TableColumn(new Text("Release Date").Centered()))
            .AddColumn(new TableColumn(new Text("Rating").Centered()))
            .AddColumn(new TableColumn(new Text("Price").Centered()));
            table.Width = 125;
            table.Title("Game Store");
            table.Caption($"(Page: {startIndex / pageSize + 1}/{(games.Count + pageSize - 1) / pageSize} | P: previous | N: next | S: search | G: add genre | B: back)");

            for (int i = startIndex; i < endIndex; i++)
            {
                Game game = games[i];
                string price = (game.Price == 0) ? price = "Free to Purchase" : price = MainMenuApp.FormatCurrencyVND(game.Price);
                table.AddRow($"\n{game.GameId}\n", $"\n{game.Name}\n", $"\n{game.ReleaseDate.ToString("dd/MM/yyyy")}\n", $"\n{game.Rating}%\n", $"\n{price}\n");
            }

            AnsiConsole.Write(table);
            Console.Write("Your choice: ");
            string choice = Console.ReadLine();
            if (int.TryParse(choice.ToString(), out int intChoice))
            {
                GameDetailsMenu(intChoice, currentPage);
            }
            else
            {
                switch (choice.ToUpper())
                {
                    case "P":
                        currentPage = Math.Max(0, currentPage - 1);
                        break;
                    case "N":
                        int maxPage = (games.Count - 1) / pageSize;
                        currentPage = Math.Min(maxPage, currentPage + 1);
                        break;
                    case "B":
                        MainMenuApp.MainMenu();
                        break;
                    case "S":
                        break;
                    case "G":
                        break;
                    default:
                        Console.Write("Invalid choice! Try again ");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }

    public static void GameDetailsMenu(int id, int currentPage)
    {
        GameBLL gameBLL = new GameBLL();
        Game game = gameBLL.SearchById(id);
        
        if (game == null)
        {
            Console.Write("Your choice is not exist! ");
            Console.ReadKey();
            GameMenu();
        }
        string stringGenres = string.Join(", ", game.GameGenres.Select(genre => genre.GenreName));
        string price = (game.Price == 0) ? price = "Free to Purchase" : price = MainMenuApp.FormatCurrencyVND(game.Price);
        while (true)
        {
            Console.Clear();
            var table = new Table();
            table.Width = 65;
            table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 Version : 0.1\nGame Details").Centered()));
            table.AddRow($"\nName: [#ffffff]{game.Name}[/]");
            table.AddRow($"Genre: [#ffffff]{stringGenres}[/]");
            table.AddRow($"Publisher: [#ffffff]{game.GamePublisher.PublisherName}[/]");
            table.AddRow($"Release Date: [#ffffff]{game.ReleaseDate.ToString("dd/MM/yyyy")}[/]");
            table.AddRow($"Size: [#ffffff]{game.Size}[/]");
            table.AddRow($"Rating: [#ffffff]{game.Rating}[/]%\n").AddRow(new Rule());
            table.AddRow($"\nPrice: [#ffffff]{price}[/]\n").AddRow(new Rule());
            table.AddRow($"\nABOUT THIS GAME\n\n[#ffffff]{game.Desc}[/]\n");
            table.Caption("(B: back | P: purchase)");
            AnsiConsole.Write(table);
            Console.Write("Your choice: ");
            if (char.TryParse(Console.ReadLine(), out char choice))
            {
                switch (choice)
                {
                    case 'B':
                    case 'b':
                        GameMenu(currentPage);
                        break;
                    case 'P':
                    case 'p':
                        break;
                    default:
                        Console.Write("Your choice is not exist! ");
                        Console.ReadKey();
                        break;
                }
            }
            else
            {
                Console.Write("Invalid choice! Try again ");
                Console.ReadKey();
            }
        }
    }
}