namespace GMA.App;
using GMA.BLL;
using Spectre.Console;
using GMA.Models;

public class GameApp
{
    public static void GameMenu(int currentPage = 0, string keywords = null, int genID = 0)
    {
        int pageSize = 10;
        GameBLL gameBLL = new GameBLL();
        List<Game> games = new List<Game>();
        GenreBLL genreBLL = new GenreBLL();
        List<Genre> genres = genreBLL.GetAll();
        string genreName = (genID == 0)? "None" : genreBLL.GetGenreNameById(genID, genres);
        games = string.IsNullOrEmpty(keywords) ? (genID != 0 ? gameBLL.SearchByGenIdKey("", genID) : gameBLL.GetAll()) : (genID != 0 ? gameBLL.SearchByGenIdKey(keywords, genID) : gameBLL.SearchByKey(keywords));
        
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
            table.Caption($"[#ffffff](Page: {startIndex / pageSize + 1}/{(games.Count + pageSize - 1) / pageSize} | P: previous | N: next | S: search | G: add genre | B: back)[/]");

            for (int i = startIndex; i < endIndex; i++)
            {
                Game game = games[i];
                string price = (game.Price == 0) ? price = "Free to Purchase" : price = MainMenuApp.FormatCurrencyVND(game.Price);
                table.AddRow($"\n{game.GameId}\n", $"\n{game.Name}\n", $"\n{game.ReleaseDate.ToString("dd/MM/yyyy")}\n", $"\n{game.Rating}%\n", $"\n{price}\n");
            }

            AnsiConsole.Write(table);
            Console.WriteLine($"\n{games.Count} results match your search.");
            Console.Write($"\nYour Choice (Selected Genre: {genreName}): ");
            string choice = Console.ReadLine();
            if (int.TryParse(choice.ToString(), out int intChoice))
            {
                List<int> gameIds = new List<int>();
                gameIds =  games.ConvertAll(game => game.GameId);

                if (gameIds.Contains(intChoice))
                {
                    GameDetailsMenu(intChoice, currentPage, keywords, genID);
                }
                else
                {
                    Console.Write("Invalid choice! Try again ");
                    Console.ReadKey();
                    GameMenu(currentPage, keywords, genID);
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
                        GameMenu(0, keywords, genID);
                        break;
                    case "G":
                        int choiceId = ChoiceGenreId();
                        GameMenu(0, keywords, choiceId);
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
        List<Genre> genres = genreBLL.GetAll().OrderBy(genre => genre.GenreId).ToList();

        var table = new Table();
        table.AddColumns("ID", "Genre Name");
        table.AddRow("0", "None");
        foreach (Genre genre in genres)
        {
            table.AddRow($"{genre.GenreId}", $"{genre.GenreName}");
        }
        AnsiConsole.Write(table);

        List<int> genreIds = new List<int>();
        genreIds = genres.ConvertAll(genre => genre.GenreId);
        genreIds.Add(0);

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
                    ChoiceGenreId();
                }
            }
            else
            {
                switch (choice2.ToUpper())
                {
                    case "B":
                        GameMenu();
                        break;
                    default:
                        Console.Write("Invalid choice! Try again ");
                        Console.ReadKey();
                        break;
                }
                ChoiceGenreId();
            }
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
            table.Caption("[#ffffff](B: back | P: purchase)[/]");
            AnsiConsole.Write(table);
            Console.Write("Your choice: ");
            if (char.TryParse(Console.ReadLine(), out char choice))
            {
                switch (choice)
                {
                    case 'B':
                    case 'b':
                        GameMenu(currentPage, keywords, genID);
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