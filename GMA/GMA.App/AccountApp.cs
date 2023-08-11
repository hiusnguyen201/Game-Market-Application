using GMA.BLL;
using GMA.Models;
using GMA.Utility;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace GMA.App;

public class AccountApp
{
    public static string patternUsername = "^[^\\s][a-zA-Z0-9_-]{3,}$";
    public static string patternRealName = "^[A-Za-z ]{2,}$";
    public static string patternEmail = @"^[^\\s][a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    public static Account accountLoggedIn = null;
    private static AccountBLL accountBLL = new AccountBLL();
    private static OrderBLL orderBLL = new OrderBLL();

    public static void MembershipMenu()
    {
        if (accountLoggedIn == null)
        {
            while (true)
            {
                Console.Clear();
                var table = new Table();
                table.Width = 45;
                table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 Version : 0.1\nManage Membership").Centered()));
                table.AddRow("1. Login");
                table.AddRow("2. Register");
                table.AddRow("0. Back to Main Menu");
                AnsiConsole.Write(table);
                Console.Write("Your Choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            LoginForm();
                            break;

                        case 2:
                            RegisterForm();
                            break;

                        case 0:
                            MainMenuApp.MainMenu();
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
        else
        {
            AccountMenu();
        }
    }

    public static void LoginForm()
    {
        Console.Clear();
        var table = new Table();
        table.Width = 45;
        table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 Version : 0.1\nLogin Form").Centered()));
        table.Caption("[#ffffff](B: back)[/]");
        AnsiConsole.Write(table);
        string username = GetAccountLogin("Username");

        string password = EncryptionAES.Encrypt(GetAccountLogin("Password"));

        Account account = accountBLL.SearchAccountLogin(username, password);
        if (account == null)
        {
            Console.Write("Please check your password and username and try again! ");
            Console.ReadKey();
            LoginForm();
        }
        else
        {
            Console.Write("Logged in successfully! ");
            Console.ReadKey();
            accountLoggedIn = account;
            accountLoggedIn.AccountOrders = orderBLL.GetAll(accountLoggedIn.AccountId);
            AccountMenu();
        }
    }

    public static void RegisterForm()
    {
        Console.Clear();
        var table = new Table();
        table.Width = 45;
        table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 Version : 0.1\nRegister Form").Centered()));
        table.Caption("[#ffffff](B: back)[/]");
        AnsiConsole.Write(table);

        string username = GetAccountRegister("Username");

        string password = EncryptionAES.Encrypt(GetAccountRegister("Password"));

        string realname = FormatString.ModifyString(GetAccountRegister("Real Name"));

        string email = GetAccountRegister("Email");

        string address = FormatString.ModifyString(GetAccountRegister("Address"));


        int result = accountBLL.Save(new Account(username, password, realname, email, address));
        if (result != 0)
        {
            Console.Write("New Account Created Successfully! ");
            Console.ReadKey();
            Console.WriteLine();
        }
        MembershipMenu();
    }

    public static void AccountMenu()
    {
        while (true)
        {
            Console.Clear();
            var table = new Table();
            table.Width = 45;
            table.AddColumn(new TableColumn(new Text($"Game Market Application\nGroup 2 - PF1122 Version 0.1\nUsername: {accountLoggedIn.Username} | Money: {FormatString.FormatCurrencyVND(accountLoggedIn.Money)}").Centered()));
            table.AddRow("1. View Profile");
            table.AddRow("2. Recharge Money");
            table.AddRow("3. View Order History");
            table.AddRow("4. Logout");
            table.AddRow("0. Back");
            AnsiConsole.Write(table);
            Console.Write("Your Choice: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        ViewProfileMenu();
                        break;

                    case 2:
                        RechargeMoneyMenu();
                        break;

                    case 3:
                        OrderHistory();
                        break;

                    case 4:
                        MainMenuApp.CheckChooseBack("Logout");
                        break;

                    case 0:
                        MainMenuApp.MainMenu();
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

    public static void ViewProfileMenu()
    {
        Console.Clear();
        var table = new Table();
        table.Width = 45;
        table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 Version : 0.1\nUser Profile").Centered()));
        table.AddRow($"\nName: [#ffffff]{accountLoggedIn.Username}[/]\n").AddRow(new Rule());
        table.AddRow($"\nReal Name: [#ffffff]{accountLoggedIn.Realname}[/]\n").AddRow(new Rule());
        table.AddRow($"\nEmail: [#ffffff]{accountLoggedIn.Email}[/]\n").AddRow(new Rule());
        table.AddRow($"\nAddress: [#ffffff]{accountLoggedIn.Address}[/]\n").AddRow(new Rule());
        table.AddRow($"\nMoney: [#ffffff]{FormatString.FormatCurrencyVND(accountLoggedIn.Money)}[/]\n").AddRow(new Rule());
        table.AddRow($"\nCreate Date: [#ffffff]{accountLoggedIn.CreateDate.ToString("dd/MM/yyyy")}[/]\n");
        AnsiConsole.Write(table);
        Console.Write("Press any key to continue! ");
        Console.ReadKey();
    }

    public static void RechargeMoneyMenu()
    {
        while (true)
        {
            Console.Clear();
            var table = new Table();
            table.Width = 45;
            table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 Version : 0.1\nRecharge Menu").Centered()));
            table.AddRow("1. Add 75.000 VND");
            table.AddRow("2. Add 150.000 VND");
            table.AddRow("3. Add 375.000 VND");
            table.AddRow("4. Add 750.000 VND");
            table.AddRow("5. Add 1.500.000 VND");
            table.AddRow("0. Back");
            AnsiConsole.Write(table);
            Console.Write("Your Choice: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        CheckPassToAddFunds(choice);
                        break;
                    case 0:
                        AccountMenu();
                        break;
                    default:
                        Console.Write("Invalid choice! Try again ");
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

    public static void CheckPassToAddFunds(int choice)
    {
        Console.Write("- Check [Password]: ");
        string password = GetPassword();
        if (password == "B" || password == "b")
        {
            RechargeMoneyMenu();
        }

        if (EncryptionAES.Encrypt(password) == accountLoggedIn.Password)
        {
            switch (choice)
            {
                case 1:
                    accountLoggedIn.Money += 75000;
                    break;

                case 2:
                    accountLoggedIn.Money += 150000;
                    break;

                case 3:
                    accountLoggedIn.Money += 375000;
                    break;

                case 4:
                    accountLoggedIn.Money += 750000;
                    break;

                case 5:
                    accountLoggedIn.Money += 1500000;
                    break;
            }

            accountBLL.UpdateMoney(accountLoggedIn.AccountId, accountLoggedIn.Money);
            Console.Write("Add Funds successful! ");
            Console.ReadKey();
            AccountMenu();
        }
        else
        {
            Console.Write("Password incorrect! ");
            Console.ReadKey();
            RechargeMoneyMenu();
        }
    }

    public static string GetAccountLogin(string text)
    {
        while (true)
        {
            Console.Write($"[{text}]: ");
            string value = text == "Password" ? GetPassword() : Console.ReadLine();

            // Check Q : quit
            if (value == "B" || value == "b")
            {
                if (!MainMenuApp.CheckChooseBack("MembershipMenu"))
                {
                    MainMenuApp.ClearCurrentConsoleLine();
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    GetAccountLogin(text);
                }
            }

            if (string.IsNullOrEmpty(value))
            {
                Console.Write($"Please Enter {text} ");
                Console.ReadKey();
                MainMenuApp.ClearCurrentConsoleLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
            else
            {
                return value;
            }
        }
    }

    public static string GetAccountRegister(string text)
    {
        while (true)
        {
            Console.Write($"- Enter {text}: ");
            string value = (text == "Password") ? GetPassword() : Console.ReadLine();

            // Check Q : quit
            if (value == "B" || value == "b")
            {
                if (!MainMenuApp.CheckChooseBack("MembershipMenu"))
                {
                    MainMenuApp.ClearCurrentConsoleLine();
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    GetAccountRegister(text);
                }
            }

            bool isValid = true;
            // Check Null 
            if (string.IsNullOrEmpty(value))
            {
                isValid = false;
                Console.Write($"{text} cannot be null! ");
                Console.ReadKey();
            }
            else
            {
                // Check Regex
                if (text == "Username")
                {
                    if (!Regex.IsMatch(value, patternUsername))
                    {
                        isValid = false;
                        Console.Write("Please enter a username that is at least 3 characters long and uses only a-z, A-Z, 0-9, _ characters");
                        Console.ReadKey();
                    }
                }
                else if (text == "Email")
                {
                    if (!Regex.IsMatch(value, patternEmail))
                    {
                        isValid = false;
                        Console.Write("Invalid Email format! ");
                        Console.ReadKey();
                    }
                }
                else if (text == "Real Name")
                {
                    if (!Regex.IsMatch(value, patternRealName))
                    {
                        isValid = false;
                        Console.Write("Please enter a real name that is at least 2 characters long and uses only a-z, A-Z, whitespace characters");
                        Console.ReadKey();
                    }
                }
                else if (text == "Username" || text == "Email")
                {

                    Account account = (text == "Username") ? accountBLL.SearchByUsername(value) : accountBLL.SearchByEmail(value);
                    if (account != null)
                    {
                        isValid = false;
                        Console.Write($"{text} already exists! ");
                        Console.ReadKey();
                    }
                }
            }
            // Check isValid
            if (isValid)
            {
                return value;
            }
            else
            {
                MainMenuApp.ClearCurrentConsoleLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
        }
    }

    public static string GetPassword()
    {
        string password = "";
        ConsoleKey key;
        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;
            if (key == ConsoleKey.Backspace && password.Length > 0)
            {
                Console.Write("\b \b");
                password = password[0..^1];
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                Console.Write("*");
                password += keyInfo.KeyChar;
            }
        }
        while (key != ConsoleKey.Enter);
        Console.Write("\n");
        return password;
    }

    public static void OrderHistory()
    {
        while (true)
        {
            Console.Clear();
            if (accountLoggedIn.AccountOrders.Count != 0)
            {
                accountLoggedIn.AccountOrders = orderBLL.GetAll(accountLoggedIn.AccountId);
            }
            var table = new Table();
            table.AddColumn("Order ID");
            table.AddColumn("Date");
            table.AddColumn("Game Name");
            table.AddColumn("Status");
            table.AddColumn("Total");
            table.Title($"{accountLoggedIn.Username.ToUpper()}'s Order History");
            table.Width = 100;
            table.Caption("[#ffffff](B: back)[/]");
            for (int i = 0; i < accountLoggedIn.AccountOrders.Count; i++)
            {
                Order order = accountLoggedIn.AccountOrders[i];
                string status = (order.Status == 1) ? "Paid" : "UnPaid";
                string listgames = string.Join("\n", order.OrderGames.Select(game => game.Name));
                table.AddRow($"\n{order.OrderId}\n", $"\n{order.OrderDate.ToString("dd/MM/yyyy")}\n", $"\n{listgames}\n", $"\n{status}\n", $"\n{FormatString.FormatCurrencyVND(order.TotalPrice)}\n");
            }
            AnsiConsole.Write(table);

            Console.Write("Your choice: ");
            if (char.TryParse(Console.ReadLine(), out char choice))
            {
                if(char.ToUpper(choice) == 'B')
                {
                    AccountMenu();
                }
                if(int.TryParse(choice.ToString(), out int choieInt))
                {
                    Order order = accountLoggedIn.AccountOrders.Find(order => order.OrderId == choieInt);
                    if (order != null)
                    {
                        OrderApp.InvoiceMenu(order, "AccountMenu");
                    }
                    else
                    {
                        Console.Write("Order Id Not Found! ");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.Write("Invalid choice! Try again ");
                    Console.ReadKey();
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
