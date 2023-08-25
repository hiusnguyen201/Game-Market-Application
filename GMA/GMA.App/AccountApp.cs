using GMA.BLL;
using GMA.Models;
using GMA.Utility;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace GMA.App;

public class AccountApp
{
    public static string patternUsername = "^[^\\s][a-zA-Z0-9_-]{3,100}$";
    public static string patternRealName = "^[A-Za-z ]{2,100}$";
    public static string patternEmail = @"^[^\\s][a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{3,100}$";
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
                            LoginForm("AccountMenu");
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

    public static void LoginForm(string text)
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
            LoginForm(text);
        }
        else
        {
            Console.Write("Logged in successfully! ");
            Console.ReadKey();
            accountLoggedIn = account;
            accountLoggedIn.AccountOrders = orderBLL.GetAll(accountLoggedIn.AccountId);
            if(text == "AccountMenu")
            {
                AccountMenu();
            }
            else if(text == "CartMenu")
            {
                OrderApp.CartMenu();
            }
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

        string username = GetAccountRegister("Username").ToLower();

        string password = EncryptionAES.Encrypt(GetAccountRegister("Password"));

        string realname = HandlingString.FormatSpecialName(HandlingString.ModifyString(GetAccountRegister("Real Name")));

        string email = GetAccountRegister("Email").ToLower();

        string address = HandlingString.FormatSpecialName(HandlingString.ModifyString(GetAccountRegister("Address")));


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
            table.AddColumn(new TableColumn(new Text($"Game Market Application\nGroup 2 - PF1122 Version 0.1\nUsername: {accountLoggedIn.Username} | Money: {HandlingString.FormatCurrencyVND(accountLoggedIn.Money)}").Centered()));
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
                        OrderApp.OrderHistory();
                        break;

                    case 4:
                        if (MainMenuApp.CheckYesNo())
                        {
                            accountLoggedIn = null;
                            MembershipMenu();
                        }
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
        table.AddRow($"\nUsername: [#ffffff]{accountLoggedIn.Username}[/]\n").AddRow(new Rule());
        table.AddRow($"\nReal Name: [#ffffff]{accountLoggedIn.Realname}[/]\n").AddRow(new Rule());
        table.AddRow($"\nEmail: [#ffffff]{accountLoggedIn.Email}[/]\n").AddRow(new Rule());
        table.AddRow($"\nAddress: [#ffffff]{accountLoggedIn.Address}[/]\n").AddRow(new Rule());
        table.AddRow($"\nMoney: [#ffffff]{HandlingString.FormatCurrencyVND(accountLoggedIn.Money)}[/]\n").AddRow(new Rule());
        table.AddRow($"\nCreated At: [#ffffff]{accountLoggedIn.CreatedAt}[/]\n").AddRow(new Rule());
        table.AddRow($"\nUpdated At: [#ffffff]{accountLoggedIn.UpdatedAt}[/]\n");
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
            table.Caption("B: back");
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
            if (MainMenuApp.CheckYesNo())
            {
                RechargeMoneyMenu();
            }
            else
            {
                CheckPassToAddFunds(choice);
            }
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

            int result = accountBLL.UpdateMoney(accountLoggedIn.AccountId, accountLoggedIn.Money);
            accountLoggedIn = accountBLL.SearchByUsername(accountLoggedIn.Username);
            
            if (result == 0)
            {
                Console.Write("Add Funds Unsuccessfully! Try Again ! ");
                Console.ReadKey();
            }
            else
            {
                Console.Write("Add Funds Successfully! ");
                Console.ReadKey();
            }

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

            if (string.IsNullOrEmpty(value))
            {
                Console.Write($"Please Enter {text} ");
                Console.ReadKey();
                MainMenuApp.ClearCurrentConsoleLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                MainMenuApp.ClearCurrentConsoleLine();
            }
            else
            {
                if (value == "B" || value == "b")
                {
                    if (MainMenuApp.CheckYesNo())
                    {
                        MembershipMenu();
                    }
                    else
                    {
                        GetAccountLogin(text);
                    }
                }
                else
                {
                    return value;
                }
            }
        }
    }

    public static string GetAccountRegister(string text)
    {
        while (true)
        {
            Console.Write($"- Enter {text}: ");
            string value = (text == "Password") ? GetPassword() : Console.ReadLine();
            bool isValid = true;

            if (string.IsNullOrEmpty(value))
            {
                isValid = false;
                Console.Write($"{text} cannot be null! ");
                Console.ReadKey();
            }
            else
            {
                if (value == "B" || value == "b")
                {
                    if (MainMenuApp.CheckYesNo())
                    {
                        MembershipMenu();
                    }
                    else
                    {
                        GetAccountRegister(text);
                    }
                }
                else
                {
                    // Check Regex
                    if (text == "Username")
                    {
                        if (!Regex.IsMatch(value, patternUsername))
                        {
                            isValid = false;
                            Console.Write("Invalid Username format");
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
                            Console.Write("Invalid Real Name format");
                            Console.ReadKey();
                        }
                    }

                    if (text == "Username" || text == "Email")
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
                MainMenuApp.ClearCurrentConsoleLine();
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
}
