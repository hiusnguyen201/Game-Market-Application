namespace GMA.App;
using GMA.BLL;
using Spectre.Console;
using GMA.Models;

public class OrderApp
{
    public static List<Game> cartGames = new List<Game>();

    public static void CartMenu()
    {
        if (cartGames.Count == 0 || cartGames == null)
        {
            Console.Write("Your Cart is empty! ");
            Console.ReadKey();
            MainMenuApp.MainMenu();
        }
        else
        {
            Console.Clear();
            string priceString; double total = 0;
            var table = new Table()
                .AddColumn(new TableColumn(new Text("Game Name").Centered()))
                .AddColumn(new TableColumn(new Text("Price").Centered()));
            
            foreach (Game game in cartGames)
            {
                priceString = (game.Price == 0) ? "Free" : MainMenuApp.FormatCurrencyVND(game.Price);
                table.AddRow($"\n{game.Name}\n", $"\n{priceString}\n");
                total += game.Price;
            }
            table.AddRow("[#ffffff]\n--- Total ---\n[/]", $"[#ffffff]\n{MainMenuApp.FormatCurrencyVND(total)}\n[/]");
            table.Width = 50;
            table.Caption("B: back | P: purchase | S: continue shopping");
            AnsiConsole.Write(table);
            
                Console.Write("Your Choice: ");
                if (char.TryParse(Console.ReadLine(), out char choice))
                {
                    switch (choice)
                    {
                        case 'B':
                        case 'b':
                            MainMenuApp.MainMenu();
                            break;

                        case 'P':
                        case 'p':
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
                        case 's':
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

    }

    public static void CheckoutMenu(double total)
    {
        while (true)
        {
            Console.Clear();
            var table = new Table();
            table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 Version : 0.1\nCheckout Buy Game").Centered()));
            table.AddRow($"Username: {AccountApp.accountLoggedIn.Username}");
            table.AddRow($"Money: {MainMenuApp.FormatCurrencyVND(AccountApp.accountLoggedIn.Money)}");
            AnsiConsole.Write(table);
            Console.Write($"[Do you want to make payment with {MainMenuApp.FormatCurrencyVND(total)}] (Y/N): ");
            if (char.TryParse(Console.ReadLine(), out char choiceCheckout))
            {
                switch (choiceCheckout)
                {
                    case 'Y':
                    case 'y':
                        if (AccountApp.accountLoggedIn.Money >= total)
                        {
                            // Deduct money
                            AccountBLL accountBLL = new AccountBLL();
                            AccountApp.accountLoggedIn.Money -= total;

                            // Create Order
                            OrderBLL orderBLL = new OrderBLL();
                            Order order = new Order(AccountApp.accountLoggedIn, total);
                            order.Status = OrderStatus.PAID;
                            int result = orderBLL.Save(order);

                            if (result != 0)
                            {
                                // Create Order Details
                                foreach (Game game in cartGames)
                                {
                                    int check = orderBLL.SaveDetails(result, game.GameId, game.Price);
                                    if(check == 0)
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
                        break;

                    case 'N':
                    case 'n':
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
    public static void InvoiceMenu(Order order)
    {

    }
}