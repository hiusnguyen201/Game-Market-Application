namespace GMA.App;
using GMA.BLL;
using Spectre.Console;
using GMA.Models;

public class GameApp
{
    public static void GameStoreMenu(int currentPage = 0, string keywords = "", int genID = 0)
    {
        Console.Clear();
        int pageSize = 10;
        GenreBLL genreBLL = new GenreBLL();
        List<Genre> genres = genreBLL.SearchByKey("");
        string genreName = (genID == 0)? "None" : genreBLL.SearchById(genID).GenreName;

        GameBLL gameBLL = new GameBLL();
        List<Game> games = (genID != 0)? gameBLL.SearchByGenIdKey(keywords, genID) : gameBLL.SearchByKey(keywords);
        
        while (true)
        {
            Console.Clear();
            int startIndex = currentPage * pageSize;
            int endIndex = Math.Min(startIndex + pageSize, games.Count);
        
            var table = new Table()
            .AddColumn(new TableColumn(new Text("ID").Centered()))
            .AddColumn(new TableColumn(new Text("Game Name").Centered()))
            .AddColumn(new TableColumn(new Text("Release Date").Centered()))
            .AddColumn(new TableColumn(new Text("Rating").Centered()))
            .AddColumn(new TableColumn(new Text("Price").Centered()));
            table.Width = 125;
            table.Title("Game Store");
            table.Caption($"[#ffffff](Page: {startIndex / pageSize + 1}/{(games.Count + pageSize - 1) / pageSize} | P: previous | N: next | S: search | G: add genre | B: back)[/]");

            for (int i = startIndex; i < endIndex; i++)
            {
                Game game = games[i];
                string priceString = (game.Price == 0) ?  "Free to Purchase" :  MainMenuApp.FormatCurrencyVND(game.Price);
                table.AddRow($"\n{game.GameId}\n", $"\n{game.Name}\n", $"\n{game.ReleaseDate.ToString("dd/MM/yyyy")}\n", $"\n{game.Rating}%\n", $"\n{priceString}\n");
            }

            AnsiConsole.Write(table);
            if(keywords == "")
            {
                Console.WriteLine($"\n{games.Count} results match your search.");
            }
            else
            {
                Console.WriteLine($"\n{games.Count} results match your search with keywords: '{keywords}'.");
            }
            Console.Write($"\nYour Choice (Selected Genre: {genreName}): ");
            string choice = Console.ReadLine();
            if (int.TryParse(choice.ToString(), out int intChoice))
            {
                List<int> gameIds = games.ConvertAll(game => game.GameId);

                if (gameIds.Contains(intChoice))
                {
                    GameDetailsMenu(intChoice, currentPage, keywords, genID);
                }
                else
                {
                    Console.Write("Invalid choice! Try again ");
                    Console.ReadKey();
                    GameStoreMenu(currentPage, keywords, genID);
                }
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
                        Console.Write("- Enter Keywords: ");
                        keywords = Console.ReadLine();
                        GameStoreMenu(0, keywords, genID);
                        break;
                    case "G":
                        int choiceId = ChoiceGenreId();
                        GameStoreMenu(0, keywords, choiceId);
                        break;
                    default:
                        Console.Write("Invalid choice! Try again ");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }

    public static int ChoiceGenreId(int choice = 0)
    {
        Console.Clear();
        GenreBLL genreBLL = new GenreBLL();

        List<Genre> genres = genreBLL.SearchByKey("").OrderBy(genre => genre.GenreId).ToList();
        List<int> genreIds = genres.ConvertAll(genre => genre.GenreId);
        genreIds.Add(0);

        var table = new Table();
        table.AddColumns("ID", "Genre Name");
        table.AddRow("0", "None");
        table.Caption("B: back");
        foreach (Genre genre in genres)
        {
            table.AddRow($"{genre.GenreId}", $"{genre.GenreName}");
        }
        AnsiConsole.Write(table);

        while (true)
        {
            Console.Write($"\nYour choice: ");
            string choice2 = Console.ReadLine();
            if (int.TryParse(choice2.ToString(), out int intChoice))
            {
                if (genreIds.Contains(intChoice))
                {
                    return intChoice;
                }
                else
                {
                    Console.Write("Invalid choice! Try again ");
                    Console.ReadKey();
                }
            }
            else
            {
                switch (choice2.ToUpper())
                {
                    case "B":
                        GameStoreMenu();
                        break;
                    default:
                        Console.Write("Invalid choice! Try again ");
                        Console.ReadKey();
                        break;
                }
            }
            ChoiceGenreId();
        }
    }

    public static void GameDetailsMenu(int id, int currentPage, string keywords, int genID)
    {
        GameBLL gameBLL = new GameBLL();
        Game game = gameBLL.SearchById(id);
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
            table.Caption("[#ffffff](B: back | A: Add to cart)[/]");
            AnsiConsole.Write(table);
            Console.Write("Your choice: ");
            if (char.TryParse(Console.ReadLine(), out char choice))
            {
                switch (choice)
                {
                    case 'B':
                    case 'b':
                        GameStoreMenu(currentPage, keywords, genID);
                        break;
                    case 'A': 
                    case 'a': 
                        OrderApp.order.OrderDetails.Add(game);
                        Console.Write("Add Game to cart successfully! ");
                        Console.ReadKey();
                        OrderApp.CartMenu();
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