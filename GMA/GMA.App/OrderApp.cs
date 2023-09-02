namespace GMA.App;
using GMA.BLL;
using GMA.Utility;
using Spectre.Console;
using GMA.Models;

public class OrderApp
{
    private static AccountBLL accountBLL = new AccountBLL();
    private static OrderBLL orderBLL = new OrderBLL();
    public static List<Game> cartGames = new List<Game>();

    public static void CartMenu()
    {
        if (cartGames.Count == 0 || cartGames == null)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Your Cart Is Empty! ");
            Console.ResetColor();
            Console.ReadKey();
            MainMenuApp.MainMenu();
            return;
        }

        Console.Clear();
        double total = CalculateTotalPrice();
        DisplayCartTable(total);

        Console.Write("Your Choice: ");
        if (char.TryParse(Console.ReadLine(), out char choice))
        {
            switch (char.ToUpper(choice))
            {
                case 'B':
                    MainMenuApp.MainMenu();
                    break;

                case 'P':
                    if (AccountApp.accountLoggedIn != null)
                    {
                        bool gameOwned = false;
                        for (int i = 0; i < cartGames.Count; i++)
                        {
                            Game gameCart = cartGames[i];
                            gameOwned = AccountApp.accountLoggedIn.AccountOrders.Any(order => order.OrderGames.Any(game => game.GameId == gameCart.GameId));
                            if (gameOwned)
                            {
                                break;
                            }
                        }

                        if (!gameOwned)
                        {
                            CheckoutMenu(total);
                        }
                        else
                        {
                            Console.Write("Your Account Already Owns Some Of The Above Games, So You Can’t Purchase Them Again ! ");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Console.Write("You Need To Login To Purchase! ");
                        Console.ReadKey();
                        AccountApp.LoginForm("CartMenu");
                    }
                    break;

                case 'S':
                    GameApp.GameStoreMenu();
                    break;

                case 'R':
                    ChoiceGameToRemoveInCart();
                    break;

                default:
                    Console.Write("Your Choice Is Not Exist! ");
                    Console.ReadKey();
                    break;
            }
        }
        else
        {
            Console.Write("Invalid Choice! Try Again! ");
            Console.ReadKey();
        }
        CartMenu();
    }

    private static double CalculateTotalPrice()
    {
        double total = 0;
        foreach (Game game in cartGames)
        {
            total += game.Price;
        }
        return total;
    }

    private static void DisplayCartTable(double total)
    {
        var formatTable = new Table();
            formatTable.Width = 70;
            formatTable.AddColumn(new TableColumn(new Text($"[Game Market Application]\nGroup 2 - PF1122 | Version : 0.1\nCart Menu").Centered()));
        AnsiConsole.Write(formatTable);
        var table = new Table()
            .AddColumn(new TableColumn(new Text("Game ID").Centered()))
            .AddColumn(new TableColumn(new Text("Game Name").Centered()))
            .AddColumn(new TableColumn(new Text("Price").Centered()));

        string priceString;
        foreach (Game game in cartGames)
        {
            priceString = (game.Price == 0) ? "Free" : HandlingString.FormatCurrencyVND(game.Price);
            if (AccountApp.accountLoggedIn != null && AccountApp.accountLoggedIn.AccountOrders != null)
            {
                bool gameOwned = AccountApp.accountLoggedIn.AccountOrders.Any(order => order.OrderGames.Any(gameOwned => gameOwned.GameId == game.GameId));
                if (gameOwned)
                {
                    priceString += " - (Purchased)";
                }
            }
            table.AddRow($"{game.GameId}", $"{game.Name}", $"{priceString}");
        }

        table.AddRow(" ", "[#ffffff]\n--- Total ---\n[/]", $"[#ffffff]\n{HandlingString.FormatCurrencyVND(total)}\n[/]");
        table.Width = 70;
        table.Caption("[#ffffff](B: Back | P: Purchase | S: Continue Shopping | R: Remove Game)[/]");
        AnsiConsole.Write(table);
    }

    public static void CheckoutMenu(double total)
    {
        while (true)
        {
            Console.Clear();
            var table = new Table();
            table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 | Version : 0.1\nCheckout Menu").Centered()));
            table.AddRow($"Username: {AccountApp.accountLoggedIn.Username}");
            table.AddRow($"Money: {HandlingString.FormatCurrencyVND(AccountApp.accountLoggedIn.Money)}");
            AnsiConsole.Write(table);

            Console.Write($"[Do You Want To Make Payment With {HandlingString.FormatCurrencyVND(total)}] (Y/N): ");
            if (char.TryParse(Console.ReadLine(), out char choiceCheckout))
            {
                switch (char.ToUpper(choiceCheckout))
                {
                    case 'Y':
                        ProcessPayment(total);
                        break;

                    case 'N':
                        CartMenu();
                        break;

                    default:
                        Console.Write("Your Choice Is Not Exist! ");
                        Console.ReadKey();
                        break;
                }
            }
            else
            {
                Console.Write("Invalid Choice! Try Again! ");
                Console.ReadKey();
            }
        }
    }

    public static void ProcessPayment(double total)
    {
        if (AccountApp.accountLoggedIn.Money >= total)
        {
            AccountApp.accountLoggedIn.Money -= total;

            Order order = new Order(AccountApp.accountLoggedIn, total);
            order.Status = OrderStatus.PAID;
            int result = orderBLL.Save(order);

            if (result != 0)
            {
                foreach (Game game in cartGames)
                {
                    int check = orderBLL.SaveDetails(result, game.GameId);
                    if (check == 0)
                    {
                        Console.Write($"Error! Create Order Details In OrderID: {result} - AccountID: {AccountApp.accountLoggedIn.AccountId}");
                        Console.ReadKey();
                        break;
                    }
                }

                int res = accountBLL.UpdateMoney(AccountApp.accountLoggedIn.AccountId, AccountApp.accountLoggedIn.Money);
                if(res != 0)
                {
                    cartGames = new List<Game>();
                    Console.Write("Make Payment Successfully! ");
                    Console.ReadKey();

                    AccountApp.accountLoggedIn.AccountOrders = orderBLL.GetAll(AccountApp.accountLoggedIn.AccountId);

                    InvoiceMenu(orderBLL.GetById(result), "CartMenu");
                }
                else
                {
                    Console.Write("Update Money Error! ");
                }
            }
            else
            {
                Console.Write("Failed To Create New Order! ");
            }

            Console.ReadKey();
            CartMenu();
        }
        else
        {
            Console.Write("Your Wallet Is Not Enough To Make Payment ! ");
            Console.ReadKey();
            CartMenu();
        }
    }

    public static void InvoiceMenu(Order order, string text)
    {
        Console.Clear();
        if (text == "CartMenu")
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("--- Thank You For Your Purchase! ---\n");
            Console.WriteLine("The Order Below Have Been Added To Your Order History.\n");
            Console.ResetColor();
        }
        var formatTable = new Table();
            formatTable.Width = 70;
            formatTable.AddColumn(new TableColumn(new Text($"[Game Market Application]\nGroup 2 - PF1122 | Version : 0.1\nOrder Details").Centered()));
        AnsiConsole.Write(formatTable);

        var tableAccountInfo = new Table()
        .AddColumn(new TableColumn(new Text("").Centered()));
        tableAccountInfo.Title("Account Info");
        tableAccountInfo.HideHeaders();
        tableAccountInfo.Width = 70;
        tableAccountInfo.AddRow($"Username: {AccountApp.accountLoggedIn.Username}\nOrder Id: {order.OrderId}\nDate: {order.CreatedAt}");
        AnsiConsole.Write(tableAccountInfo);

        var tableOrderDetails = new Table()
        .AddColumn(new TableColumn(new Text("Game Name").Centered()))
        .AddColumn(new TableColumn(new Text("Price").Centered()));
        for (int i = 0; i < order.OrderGames.Count; i++)
        {
            Game game = order.OrderGames[i];
            tableOrderDetails.AddRow($"{game.Name}", $"{HandlingString.FormatCurrencyVND(game.Price)}");
        }
        tableOrderDetails.AddRow(new Rule());
        tableOrderDetails.Title("List Game");
        tableOrderDetails.Width = 70;
        tableOrderDetails.AddRow("\n--- Total ---\n", $"\n{HandlingString.FormatCurrencyVND(order.TotalPrice)}\n");
        AnsiConsole.Write(tableOrderDetails);
        
        if (text == "CartMenu")
        {
            Console.Write("\nPress Any Key To Back To Main Menu ! ");
            Console.ReadKey();
            MainMenuApp.MainMenu();
        }
        else if (text == "AccountMenu")
        {
            Console.Write("\nPress Any Key To Back To Order History ! ");
            Console.ReadKey();
            OrderHistory();
        }
    }

    public static void ChoiceGameToRemoveInCart()
    {
        while (true)
        {
            Console.Write("Enter Game ID You Want To Delete: ");
            string choice = Console.ReadLine();
            if (choice.ToUpper() == "B")
            {
                if(MainMenuApp.CheckYesNo())
                {
                    CartMenu();
                }
                else
                {
                    ChoiceGameToRemoveInCart();
                }
            }

            if (int.TryParse(choice.ToString(), out int choiceID))
            {
                Game game = cartGames.Find(game => game.GameId == choiceID);
                if (game != null)
                {
                    if(MainMenuApp.CheckYesNo())
                    {
                        cartGames.Remove(game);
                        CartMenu();
                    }
                    else
                    {
                        CartMenu();
                    }
                }
                else
                {
                    Console.Write("Game Id Not Found! ");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.Write("Invalid Choice! Try Again! ");
                Console.ReadKey();
            }
            MainMenuApp.ClearCurrentConsoleLine();
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            MainMenuApp.ClearCurrentConsoleLine();
        }
    }

    public static void OrderHistory()
    {
        while (true)
        {
            AccountApp.accountLoggedIn.AccountOrders = orderBLL.GetAll(AccountApp.accountLoggedIn.AccountId);

            if (AccountApp.accountLoggedIn.AccountOrders.Count == 0)
            {
                Console.Write("You Don't Have Any Orders In Your Order History! ");
                Console.ReadKey();
                AccountApp.AccountMenu();
            }
            Console.Clear();

            var formatTable = new Table();
            formatTable.Width = 100;
            formatTable.AddColumn(new TableColumn(new Text($"[Game Market Application]\nGroup 2 - PF1122 | Version : 0.1\n{AccountApp.accountLoggedIn.Username.ToUpper()}'s Order History").Centered()));
            AnsiConsole.Write(formatTable);

            var table = new Table();
            table.AddColumn("Order ID");
            table.AddColumn("Date");
            table.AddColumn("Game Name");
            table.AddColumn("Status");
            table.AddColumn("Total");
            table.Width = 100;
            table.Caption("[#ffffff](B: Back)[/]");
            for (int i = 0; i < AccountApp.accountLoggedIn.AccountOrders.Count; i++)
            {
                Order order = AccountApp.accountLoggedIn.AccountOrders[i];
                string status = (order.Status == 1) ? "Paid" : "UnPaid";
                string listgames = string.Join("\n", order.OrderGames.Select(game => game.Name));
                table.AddRow($"{order.OrderId}", $"{order.CreatedAt.ToString("dd/MM/yyyy")}", $"{listgames}", $"{status}", $"{HandlingString.FormatCurrencyVND(order.TotalPrice)}");
            }
            AnsiConsole.Write(table);

            Console.Write("Your Choice: ");
            string choice = HandlingString.ModifyString(Console.ReadLine());
            if (choice.ToUpper() == "B")
            {
                AccountApp.AccountMenu();
            }
                
            if (int.TryParse(choice.ToString(), out int choieInt))
            {
                Order order = AccountApp.accountLoggedIn.AccountOrders.Find(order => order.OrderId == choieInt);
                if (order != null)
                {
                    InvoiceMenu(order, "AccountMenu");
                }
                else
                {
                    Console.Write("Order Id Not Found! ");
                    Console.ReadKey();
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