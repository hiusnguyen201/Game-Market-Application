namespace GMA.App;
using GMA.BLL;
using GMA.Utility;
using Spectre.Console;
using GMA.Models;

public class GameApp
{
    private static GameBLL gameBLL = new GameBLL();
    private static GenreBLL genreBLL = new GenreBLL();
    private const int PageSize = 10;

    public static void GameStoreMenu(int currentPage = 0, string keywords = "", int genID = 0)
    {
        Console.Clear();
        int pageSize = 10;

        List<Genre> genres = genreBLL.SearchByKey("");
        string genreName = (genID == 0) ? "None" : genreBLL.SearchById(genID).GenreName;

        List<Game> games = (genID != 0) ? gameBLL.SearchByGenIdKey(keywords, genID) : gameBLL.SearchByKey(keywords);

        while (true)
        {
            Console.Clear();
            int startIndex = currentPage * pageSize;
            int endIndex = Math.Min(startIndex + pageSize, games.Count);

            DisplayGameTable(games, startIndex, endIndex, pageSize, genreName, keywords);

            if (keywords == "")
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
                if (games.Any(game => game.GameId == intChoice))
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
                        if(MainMenuApp.CheckYesNo())
                        {
                            MainMenuApp.MainMenu();
                        }
                        else
                        {
                            GameStoreMenu(currentPage, keywords, genID);
                        }
                        break;
                    case "S":
                        Console.Write("- Enter Keywords: ");
                        keywords = HandlingString.ModifyString(Console.ReadLine());
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

    private static void DisplayGameTable(List<Game> games, int startIndex, int endIndex, int pageSize, string genreName, string keywords)
    {
        var table = new Table()
            .AddColumn(new TableColumn(new Text("ID").Centered()))
            .AddColumn(new TableColumn(new Text("Game Name").Centered()))
            .AddColumn(new TableColumn(new Text("Release Date").Centered()))
            .AddColumn(new TableColumn(new Text("Rating").Centered()))
            .AddColumn(new TableColumn(new Text("Price").Centered()));
        table.Width = 125;
        table.Title("Game Store");
        table.Caption($"[#ffffff](Page: {startIndex / pageSize + 1}/{(games.Count + pageSize - 1) / pageSize} | P: previous | N: next | S: search | G: add genre | B: back)[/]");

        string priceString;
        for (int i = startIndex; i < endIndex; i++)
        {
            Game game = games[i];
            Game gameInCart = OrderApp.cartGames.Find(gameInCart => gameInCart.GameId == game.GameId);
            priceString = (game.Price == 0) ? "Free to Purchase" : HandlingString.FormatCurrencyVND(game.Price);
            if (AccountApp.accountLoggedIn != null && AccountApp.accountLoggedIn.AccountOrders != null)
            {
                bool gameOwned = AccountApp.accountLoggedIn.AccountOrders
                .Any(order => order.OrderGames.Any(gameOwned => gameOwned.GameId == game.GameId));
                if (gameOwned)
                {
                    priceString += " - (Purchased)";
                }
            }
            else if(gameInCart != null)
            {
                priceString += " - (In Cart)";
            }
            table.AddRow($"\n{game.GameId}\n", $"\n{game.Name}\n", $"\n{game.ReleaseDate.ToString("dd/MM/yyyy")}\n", $"\n{game.Rating}%\n", $"\n{priceString}\n");
        }

        AnsiConsole.Write(table);
    }

    public static int ChoiceGenreId(int choice = 0)
    {
        Console.Clear();
        List<Genre> genres = genreBLL.SearchByKey("").OrderBy(genre => genre.GenreId).ToList();

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
            Console.Write($"Your choice: ");
            string choice2 = Console.ReadLine();
            if (int.TryParse(choice2, out int intChoice))
            {
                if (intChoice == 0 || genres.Any(genre => genre.GenreId == intChoice))
                {
                    return intChoice;
                }
                else
                {
                    Console.Write("Invalid choice! Try again ");
                    Console.ReadKey();
                }
            }
            else if (choice2.ToUpper() == "B")
            {
                if(MainMenuApp.CheckYesNo())
                {
                    GameStoreMenu();
                }
                else
                {
                    ChoiceGenreId();
                }
            }
            
            Console.Write("Invalid choice! Try again ");
            Console.ReadKey();
            MainMenuApp.ClearCurrentConsoleLine();
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            MainMenuApp.ClearCurrentConsoleLine();
        }
    }

    public static void GameDetailsMenu(int id, int currentPage, string keywords, int genID)
    {
        Game game = gameBLL.SearchById(id);
        string stringGenres = string.Join(", ", game.GameGenres.Select(genre => genre.GenreName));
        string price = (game.Price == 0) ? price = "Free to Purchase" : price = HandlingString.FormatCurrencyVND(game.Price);

        Game gameInCart = OrderApp.cartGames.Find(game => game.GameId == id);

        bool gameOwned = false; string capString = "";
        if (AccountApp.accountLoggedIn != null && AccountApp.accountLoggedIn.AccountOrders != null)
        {
            gameOwned = AccountApp.accountLoggedIn.AccountOrders
                .Any(order => order.OrderGames.Any(gameOwned => gameOwned.GameId == game.GameId));
        }

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
            
            if (gameInCart != null)
            {
                table.Caption("[#ffffff](B: back | In cart)[/]");
            }
            else if (gameOwned)
            {
                table.Caption("[#ffffff](B: back | Purchased)[/]");
            }
            else
            {
                table.Caption("[#ffffff](B: back | A: add to cart)[/]");
            }

            AnsiConsole.Write(table);
            Console.Write("Your choice: ");
            if (char.TryParse(Console.ReadLine(), out char choice))
            {
                if (gameInCart != null || gameOwned)
                {
                    switch (char.ToUpper(choice))
                    {
                        case 'B':
                            GameStoreMenu(currentPage, keywords, genID);
                            break;

                        default:
                            Console.Write("Your choice is not exist! ");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    switch (char.ToUpper(choice))
                    {
                        case 'B':
                            GameStoreMenu(currentPage, keywords, genID);
                            break;
                        case 'A':
                            if (gameInCart != null)
                            {
                                OrderApp.CartMenu();
                            }
                            OrderApp.cartGames.Add(game);
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
            }
            else
            {
                Console.Write("Invalid choice! Try again ");
                Console.ReadKey();
            }
        }
    }
}