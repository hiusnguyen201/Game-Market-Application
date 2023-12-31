using GMA.BLL;
using GMA.Models;
using GMA.Utility;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace GMA.App;

public class AccountApp
{
    protected static string patternUsername = "^[^\\s][a-zA-Z0-9_-]{3,100}$";
    protected static string patternPassword = @"^.{3,50}$";
    protected static string patternRealName = "^[A-Za-z ]{2,100}$";
    protected static string patternEmail = @"^[^\\s][a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{3,100}$";
    private static AccountBLL accountBLL = new AccountBLL();
    
    public static void AuthenticationMenu()
    {
        while (true)
        {
            Console.Clear();
            var table = new Table();
            table.Width = 45;
            table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 | Version : 0.1\nAuthentication Menu").Centered()));
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
                        Console.Write("Your choice Is Not Exist! ");
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

    public static void AccountMenu()
    {

        Console.Clear();
        Account account = accountBLL.GetAccountLoggedIn();
        if(account == null)
        {
            AuthenticationMenu();
        }

        while (true)
        {
            Console.Clear();
            var table = new Table();
            table.Width = 45;
            table.AddColumn(new TableColumn(new Text($"[Game Market Application]\nGroup 2 - PF1122 | Version : 0.1\nAccount Menu\nUsername: {account.Username} | Money: {HandlingString.FormatCurrencyVND(account.Money)}").Centered()));
            table.AddRow("1. View Profile");
            table.AddRow("2. Recharge Money");
            table.AddRow("3. View Order History");
            table.AddRow("4. Logout");
            table.AddRow("0. Back to Main Menu");
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
                            accountBLL.SetAccountLoggedIn(new Account());
                            AuthenticationMenu();
                        }
                        break;

                    case 0:
                        MainMenuApp.MainMenu();
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

    public static void LoginForm(string text)
    {

        Console.Clear();
        var table = new Table();
        table.Width = 45;
        table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 | Version : 0.1\nLogin Form").Centered()));
        table.Caption("[#ffffff](B: Back)[/]");
        AnsiConsole.Write(table);
        string username = GetAccountLogin("Username");

        string password = EncryptionAES.Encrypt(GetAccountLogin("Password"));

        Account account = accountBLL.SearchAccountLogin(username, password);
        if (account == null)
        {
            Console.Write("Please Check Your Password And Username And Try Again! ");
            Console.ReadKey();
            LoginForm(text);
        }
        else
        {
            Console.Write("Logged In Successfully! ");
            Console.ReadKey();
            accountBLL.SetAccountLoggedIn(account);
            if (text == "AccountMenu")
            {
                AccountMenu();
            }
            else if (text == "CartMenu")
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
        table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 | Version : 0.1\nRegister Form").Centered()));
        table.Caption("[#ffffff](B: Back)[/]");
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
        AuthenticationMenu();
    }

    public static void ViewProfileMenu()
    {
        Console.Clear();
        var table = new Table();
        table.Width = 45;
        table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 | Version : 0.1\nUser Profile").Centered()));
        table.AddRow($"\nUsername: [#ffffff]{accountBLL.GetAccountLoggedIn().Username}[/]\n").AddRow(new Rule());
        table.AddRow($"\nReal Name: [#ffffff]{accountBLL.GetAccountLoggedIn().Realname}[/]\n").AddRow(new Rule());
        table.AddRow($"\nEmail: [#ffffff]{accountBLL.GetAccountLoggedIn().Email}[/]\n").AddRow(new Rule());
        table.AddRow($"\nAddress: [#ffffff]{accountBLL.GetAccountLoggedIn().Address}[/]\n").AddRow(new Rule());
        table.AddRow($"\nMoney: [#ffffff]{HandlingString.FormatCurrencyVND(accountBLL.GetAccountLoggedIn().Money)}[/]\n").AddRow(new Rule());
        table.AddRow($"\nCreated At: [#ffffff]{accountBLL.GetAccountLoggedIn().CreatedAt}[/]\n").AddRow(new Rule());
        table.AddRow($"\nUpdated At: [#ffffff]{accountBLL.GetAccountLoggedIn().UpdatedAt}[/]\n");
        AnsiConsole.Write(table);
        Console.Write("\nPress Any Key To Back To Account Menu! ");
        Console.ReadKey();
    }

    public static void RechargeMoneyMenu()
    {
        while (true)
        {
            Console.Clear();
            var table = new Table();
            table.Width = 45;
            table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 | Version : 0.1\nRecharge Money Menu").Centered()));
            table.AddRow("1. Add 75.000 VND");
            table.AddRow("2. Add 150.000 VND");
            table.AddRow("3. Add 375.000 VND");
            table.AddRow("4. Add 750.000 VND");
            table.AddRow("5. Add 1.500.000 VND");
            table.AddRow("0. Back to Account Menu");
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
                        Console.Write("Invalid Choice! Try Again! ");
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

    public static void CheckPassToAddFunds(int choice)
    {
        Console.Write("- Input Password (B: Back): ");
        string password = HandlingString.GetPassword();
        if(string.IsNullOrEmpty(password))
        {
            Console.Write("Please enter password! ");
            Console.ReadKey();
            MainMenuApp.ClearCurrentConsoleLine();
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            MainMenuApp.ClearCurrentConsoleLine();
            CheckPassToAddFunds(choice);
        }

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

        if (EncryptionAES.Encrypt(password) == accountBLL.GetAccountLoggedIn().Password)
        {
            double currentMoney = accountBLL.GetAccountLoggedIn().Money;

            int result = accountBLL.UpdateMoney(choice);
            accountBLL.SetAccountLoggedIn(accountBLL.SearchByUsername(accountBLL.GetAccountLoggedIn().Username));

            if (result == 0)
            {
                Console.Write("Your Wallet Add Funds Unsuccessfully! Try Again ! ");
                Console.ReadKey();
            }
            else
            {
                Console.Write($"Your Wallet Add Funds Successfully with {HandlingString.FormatCurrencyVND(accountBLL.GetAccountLoggedIn().Money - currentMoney)}! ");
                Console.ReadKey();
            }

            AccountMenu();
        }
        else
        {
            Console.Write("Password Incorrect! ");
            Console.ReadKey();
            MainMenuApp.ClearCurrentConsoleLine();
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            MainMenuApp.ClearCurrentConsoleLine();
            CheckPassToAddFunds(choice);
        }
    }

    public static string GetAccountLogin(string text)
    {
        while (true)
        {
            Console.Write($"[{text}]: ");
            string value = text == "Password" ? HandlingString.GetPassword() : Console.ReadLine();

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
                        AuthenticationMenu();
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
            string value = (text == "Password") ? HandlingString.GetPassword() : Console.ReadLine();
            bool isValid = true;

            if (string.IsNullOrEmpty(value))
            {
                isValid = false;
                Console.Write($"Please Enter {text} ");
                Console.ReadKey();
            }
            else
            {
                if (value == "B" || value == "b")
                {
                    if (MainMenuApp.CheckYesNo())
                    {
                        AuthenticationMenu();
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
                            Console.Write("The Username Must Follow A Sequence Of Letters, Can Contain Digits, Underscores, Hyphens And Has A Length Between 2 And 100 Characters ");
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
                            Console.Write("The Real Name Must Consist Of Letters And Spaces And Has A Length Between 3 And 100 Characters ");
                            Console.ReadKey();
                        }
                    }
                    else if (text == "Password")
                    {
                        if (!Regex.IsMatch(value, patternPassword))
                        {
                            isValid = false;
                            Console.Write("The Password Must Has A Length Between 3 And 50 Characters");
                            Console.ReadKey();
                        }
                    }

                    if (text == "Username" || text == "Email")
                    {
                        Account account = (text == "Username") ? accountBLL.SearchByUsername(value) : accountBLL.SearchByEmail(value);
                        if (account != null)
                        {
                            isValid = false;
                            Console.Write($"{text} Already Exists! ");
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
}