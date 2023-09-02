namespace GMA.App;
using GMA.BLL;
using GMA.Utility;
using Spectre.Console;
using GMA.Models;

public class GameApp
{
    private static GameBLL gameBLL = new GameBLL();
    private static GenreBLL genreBLL = new GenreBLL();

    public static void GameStoreMenu(int currentPage = 0, string kw = "", int gid = 1)
    {
        Console.Clear();
        int pageSize = 10;

        string genreName = genreBLL.SearchById(gid).GenreName;

        List<Game> games = gameBLL.SearchByGenIdKey(kw, gid);

        while (true)
        {
            Console.Clear();
            int startIndex = currentPage * pageSize;
            int endIndex = Math.Min(startIndex + pageSize, games.Count);

            DisplayListGameTable(games, startIndex, endIndex, pageSize);

            if (kw == "")
            {
                if(gid != 1)
                {
                    Console.WriteLine($"\n{games.Count} Results Match Your Search With Genre: '{genreName}'.");
                }
                else
                {
                    Console.WriteLine($"\n{games.Count} Results Match Your Search.");
                }
            }
            else
            {
                if(gid != 1)
                {
                    Console.WriteLine($"\n{games.Count} Results Match Your Search With Keywords: '{kw}' and Genre: '{genreName}'.");
                }
                else
                {
                    Console.WriteLine($"\n{games.Count} Results Match Your Search With Keywords: '{kw}'.");
                }
            }

            Console.Write($"\nYour Choice (Selected Genre: {genreName}): ");
            string choice = Console.ReadLine();
            if (int.TryParse(choice.ToString(), out int intChoice))
            {
                if (games.Any(game => game.GameId == intChoice))
                {
                    GameDetailsMenu(intChoice, currentPage, kw, gid);
                }
                else
                {
                    Console.Write("Invalid Choice! Try Again! ");
                    Console.ReadKey();
                    GameStoreMenu(currentPage, kw, gid);
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
                        kw = HandlingString.ModifyString(Console.ReadLine());
                        GameStoreMenu(0, kw, gid);
                        break;
                    case "G":
                        int choiceId = ChoiceGenreId();
                        GameStoreMenu(0, kw, choiceId);
                        break;
                    default:
                        Console.Write("Invalid Choice! Try Again! ");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }

    private static void DisplayListGameTable(List<Game> games, int startIndex, int endIndex, int pageSize)
    {
        var formatTable = new Table();
        formatTable.Width = 125;
        formatTable.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 | Version : 0.1\nList Game").Centered()));
        AnsiConsole.Write(formatTable);

        var table = new Table()
            .AddColumn(new TableColumn(new Text("ID").Centered()))
            .AddColumn(new TableColumn(new Text("Game Name").Centered()))
            .AddColumn(new TableColumn(new Text("Release Date").Centered()))
            .AddColumn(new TableColumn(new Text("Rating").Centered()))
            .AddColumn(new TableColumn(new Text("Price").Centered()));
        table.Width = 125;
        int pageNum = (games.Count == 0)? 0 : (startIndex / pageSize + 1);
        table.Caption($"[#ffffff](Page: {pageNum}/{(games.Count + pageSize - 1) / pageSize} | P: Previous | N: Next | S: Search | G: Add Genre | B: Back)[/]");

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
                else if(gameInCart != null)
                {
                    priceString += " - (In Cart)";
                }
            }
            else if(gameInCart != null)
            {
                priceString += " - (In Cart)";
            }
            table.AddRow($"{game.GameId}", $"{game.Name}", $"{game.CreatedAt.ToString("dd/MM/yyyy")}", $"{game.Rating}%", $"{priceString}");
        }

        AnsiConsole.Write(table);
    }

    public static int ChoiceGenreId()
    {
        Console.Clear();
        List<Genre> genres = genreBLL.GetAll().OrderBy(genre => genre.GenreId).ToList();

        var formatTable = new Table();
        formatTable.Width = 45;
        formatTable.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 | Version : 0.1\nList Genres").Centered()));
        AnsiConsole.Write(formatTable);

        var table = new Table();
        table.AddColumns("ID", "Genre Name");
        table.Caption("[#ffffff](B: Back)[/]");
        table.Width = 45;
        foreach (Genre genre in genres)
        {
            table.AddRow($"{genre.GenreId}", $"{genre.GenreName}");
        }
        AnsiConsole.Write(table);

        while (true)
        {
            Console.Write($"Your Choice: ");
            string choice2 = Console.ReadLine();
            if (int.TryParse(choice2, out int intChoice))
            {
                if (genres.Any(genre => genre.GenreId == intChoice))
                {
                    return intChoice;
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
            
            Console.Write("Invalid Choice! Try Again! ");
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
            table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 | Version : 0.1\nGame Details").Centered()));
            table.AddRow($"\nName: [#ffffff]{game.Name}[/]");
            table.AddRow($"Genre: [#ffffff]{stringGenres}[/]");
            table.AddRow($"Publisher: [#ffffff]{game.GamePublisher.PublisherName}[/]");
            table.AddRow($"Release Date: [#ffffff]{game.CreatedAt.ToString("dd/MM/yyyy")}[/]");
            table.AddRow($"Size: [#ffffff]{game.Size}[/]");
            table.AddRow($"Rating: [#ffffff]{game.Rating}[/]%\n").AddRow(new Rule());
            table.AddRow($"\nPrice: [#ffffff]{price}[/]\n").AddRow(new Rule());
            table.AddRow($"\nAbout This Game\n\n[#ffffff]{game.Desc}[/]\n");
            
            if (gameInCart != null)
            {
                table.Caption("[#ffffff](B: Back | In cart)[/]");
            }
            else if (gameOwned)
            {
                table.Caption("[#ffffff](B: Back | Purchased)[/]");
            }
            else
            {
                table.Caption("[#ffffff](B: Back | A: Add to cart)[/]");
            }

            AnsiConsole.Write(table);
            Console.Write("Your Choice: ");
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
                            Console.Write("Your Choice Is Not Exist! ");
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
                            Console.Write("Add Game To Cart Successfully! ");
                            Console.ReadKey();
                            OrderApp.CartMenu();
                            break;
                        default:
                            Console.Write("Your Choice Is Not Exist! ");
                            Console.ReadKey();
                            break;
                    }
                }
            }
            else
            {
                Console.Write("Invalid Choice! Try Again! ");
                Console.ReadKey();
            }
        }
    }
}