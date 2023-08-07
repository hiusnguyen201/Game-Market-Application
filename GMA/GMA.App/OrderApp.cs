namespace GMA.App;
using GMA.BLL;
using Spectre.Console;
using GMA.Models;

public class OrderApp
{
    public static Order order = new Order();
    public static void CartMenu()
    {
        if(order.OrderDetails.Count == 0)
        {
            Console.Write("Your Cart is empty! ");
            Console.ReadKey();
            MainMenuApp.MainMenu();
        }
        else 
        {
            string priceString;
            double total = 0;
            var table = new Table()
            .AddColumn(new TableColumn(new Text("Game Name").Centered()))
            .AddColumn(new TableColumn(new Text("Price").Centered()));
            foreach(Game game in order.OrderDetails)
            {
                priceString = (game.Price == 0) ? "Free" : MainMenuApp.FormatCurrencyVND(game.Price);
                table.AddRow($"\n{game.Name}\n", $"\n{priceString}\n");
                total += game.Price;
            }
            table.AddRow("[#ffffff]\n--- Total ---\n[/]", $"[#ffffff]\n{MainMenuApp.FormatCurrencyVND(total)}\n[/]");
            table.Width = 50;
            table.Caption("B: Back | P: Purchase | S: Continue Shopping");
            AnsiConsole.Write(table);
            Console.Write("Your Choice: ");
            if(char.TryParse(Console.ReadLine(), out char choice))
            {
                switch (choice)
                {
                    case 'B': 
                    case 'b':
                        MainMenuApp.MainMenu(); 
                    break;

                    case 'P':
                    case 'p':
                        if(AccountApp.accountLoggedIn != null)
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
                }
            }
        }
        
    }

        public static void CheckoutMenu(double money)
        {
            var table = new Table();
             table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 Version : 0.1\nCheckout Buy Game").Centered()));
             table.AddRow($"Username: {AccountApp.accountLoggedIn.Username}");
             table.AddRow($"Money: {AccountApp.accountLoggedIn.Money}");   
             AnsiConsole.Write(table);
             Console.WriteLine($"Do you want to make payment with {MainMenuApp.FormatCurrencyVND(money)} (y/n): ");
             if(char.TryParse(Console.ReadLine(), out char choiceCheckout))
             {
                switch (choiceCheckout)
                {
                    case 'Y':
                    case 'y':
                        if(AccountApp.accountLoggedIn.Money >= money)
                        {
                            Order order = new Order(AccountApp.accountLoggedIn, money);
                            AccountApp.accountLoggedIn.Money -= money;
                            order.Status = OrderStatus.PAID;
                            OrderBLL orderBLL = new OrderBLL();
                            orderBLL.Save(order);
                            Console.Write("Make Payment successfully! ");
                            Console.ReadKey();
                            InvoiceMenu(order);
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
                }
             }
        }
        public static void InvoiceMenu(Order order)
        {
            var table = new Table()
             .AddColumn(new TableColumn(new Text("Game Name").Centered()))
            .AddColumn(new TableColumn(new Text("Price").Centered()));
            foreach(Game order.OrderDetails in AccountApp.accountLoggedIn.AccountOrders)
            {
                table.AddRow($"\n{}\n", $"\n{priceString}\n"); 
            }   
        }
}