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
            Console.Write("Your Cart is empty! ");
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
                        CheckoutMenu(total);
                    }
                    else
                    {
                        Console.Write("You need to Login to purchase! ");
                        Console.ReadKey();
                        AccountApp.LoginForm();
                    }
                    break;

                case 'S':
                    GameApp.GameStoreMenu();
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
        var table = new Table()
            .AddColumn(new TableColumn(new Text("Game Name").Centered()))
            .AddColumn(new TableColumn(new Text("Price").Centered()));

        foreach (Game game in cartGames)
        {
            string priceString = (game.Price == 0) ? "Free" : FormatString.FormatCurrencyVND(game.Price);
            table.AddRow($"\n{game.Name}\n", $"\n{priceString}\n");
        }

        table.AddRow("[#ffffff]\n--- Total ---\n[/]", $"[#ffffff]\n{FormatString.FormatCurrencyVND(total)}\n[/]");
        table.Width = 70;
        table.Caption("B: back | P: purchase | S: continue shopping");
        AnsiConsole.Write(table);
    }

    public static void CheckoutMenu(double total)
    {
        while (true)
        {
            Console.Clear();
            var table = new Table();
            table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 Version : 0.1\nCheckout Buy Game").Centered()));
            table.AddRow($"Username: {AccountApp.accountLoggedIn.Username}");
            table.AddRow($"Money: {FormatString.FormatCurrencyVND(AccountApp.accountLoggedIn.Money)}");
            AnsiConsole.Write(table);

            Console.Write($"[Do you want to make payment with {FormatString.FormatCurrencyVND(total)}] (Y/N): ");
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

    public static void ProcessPayment(double total)
    {
        if (AccountApp.accountLoggedIn.Money >= total)
        {
            // Deduct money
            AccountApp.accountLoggedIn.Money -= total;

            // Create Order
            Order order = new Order(AccountApp.accountLoggedIn, total);
            order.Status = OrderStatus.PAID;
            int result = orderBLL.Save(order);

            if (result != 0)
            {
                // Create Order Details
                foreach (Game game in cartGames)
                {
                    int check = orderBLL.SaveDetails(result, game.GameId);
                    if (check == 0)
                    {
                        Console.Write($"Error! Create Order Deatils In OrderID: {result} - AccountID: {AccountApp.accountLoggedIn.AccountId}");
                        Console.ReadKey();
                        break;
                    }
                }

                // Update Money
                accountBLL.UpdateMoney(AccountApp.accountLoggedIn.AccountId, AccountApp.accountLoggedIn.Money);

                // Reset Cart
                cartGames = new List<Game>();

                Console.Write("Make Payment successfully! ");
                Console.ReadKey();

                //Print Invoice 

                MainMenuApp.MainMenu();
            }

            Console.Write("Failed to Create new Order! ");
            Console.ReadKey();
            CartMenu();
        }
        else
        {
            Console.Write("Your Balance is not enough to make payment! ");
            Console.ReadKey();
            CartMenu();
        }
    }

    public static void InvoiceMenu(Order order)
    {

    }
}