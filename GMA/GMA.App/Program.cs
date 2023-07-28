namespace GMA.App;

using GMA.BLL;
using GMA.Models;
using Spectre.Console;
using System.Text.RegularExpressions;

public class Menu
{
    public static string patternUsername = "^[^\\s][a-zA-Z0-9_-]{3,16}$";
    public static string patternEmail = @"^[^\\s][a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    public static Account accountLoggedIn = null;

    public static void Main(string[] args)
    {
        MainMenu();
    }

    public static void MainMenu()
    {
        while (true)
        {
            Console.Clear();
            var table = new Table();
            table.AddColumn(new TableColumn(new Text("Game Market Application\nGroup 2 - PF1122 Version 0.1\nWelcome to Game Market").Centered()));
            if (accountLoggedIn == null)
            {
                table.AddRow("1. Membership and Profile");
            }
            else
            {
                table.AddRow("1. Manage Account");
            }
            table.AddRow("2. Search Game");
            table.AddRow("3. View Cart");
            table.AddRow("0. Exit");
            AnsiConsole.Write(table);
            Console.Write("Your Choice: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        MembershipMenu();
                        break;

                    case 2:
                        AccountBLL accountBLL = new AccountBLL();
                        List<Account> accounts = new List<Account>();
                        accounts = accountBLL.DisplayAllAccount();
                        foreach (Account account in accounts)
                        {
                            Console.WriteLine($"{account.AccountId} \t {account.Username} \t {account.Password} \t {account.Realname}");
                        }
                        Console.ReadKey();
                        break;

                    case 3:
                        break;

                    case 0:
                        Environment.Exit(1);
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

    public static void MembershipMenu()
    {
        if (accountLoggedIn == null)
        {
            while (true)
            {
                Console.Clear();
                var table = new Table();
                table.AddColumn(new TableColumn(new Text("Game Market Application\nGroup 2 - PF1122 Version 0.1\nMembership and Profile").Centered()));
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
                            MainMenu();
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
        table.AddColumn(new TableColumn(new Text("Game Market Application\nGroup 2 - PF1122 Version 0.1\nLogin Form (B: back)").Centered()));
        AnsiConsole.Write(table);
        string username = GetStringLoginForm("Username");

        string password = GetStringLoginForm("Password");

        AccountBLL accountBLL = new AccountBLL();
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
            AccountMenu();
        }
    }

    public static void AccountMenu()
    {
        while (true)
        {
            Console.Clear();
            var table = new Table();
            table.AddColumn(new TableColumn(new Text($"Game Market Application\nGroup 2 - PF1122 Version 0.1\nUsername: {accountLoggedIn.Username} | Money: {accountLoggedIn.Money} $").Centered()));
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
                        RechargeMenu();
                        break;

                    case 3:
                        break;

                    case 4:
                        CheckChooseBack("Logout");
                        break;

                    case 0:
                        MainMenu();
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

    public static void RegisterForm()
    {
        Console.Clear();
        var table = new Table();
        table.AddColumn(new TableColumn(new Text("Game Market Application\nGroup 2 - PF1122 Version 0.1\nRegister Form (B: back)").Centered()));
        AnsiConsole.Write(table);

        string username = GetStringRegisterForm("Username");

        string password = GetStringRegisterForm("Password");

        string realname = ModifyString(GetStringRegisterForm("Real Name"));

        string email = GetStringRegisterForm("Email");

        string address = ModifyString(GetStringRegisterForm("Address"));

        AccountBLL accountBLL = new AccountBLL();
        int result = accountBLL.Save(new Account(username, password, realname, email, address));
        if (result != 0)
        {
            Console.Write("New Account Created Successfully! ");
            Console.ReadKey();
        }

        CheckContinue("CreateAccount");
    }

    public static void CheckContinue(string text)
    {
        if (text == "CreateAccount")
        {
            while (true)
            {
                Console.Write("Do you want to continue (Y/N): ");
                if (char.TryParse(Console.ReadLine(), out char choice))
                {
                    switch (choice)
                    {
                        case 'y':
                        case 'Y':
                            RegisterForm();
                            return;
                        case 'n':
                        case 'N':
                            MembershipMenu();
                            return;
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
                ClearCurrentConsoleLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
        }
    }

    public static bool CheckChooseBack(string text)
    {
        while (true)
        {
            Console.Write("Are you sure (Y/N): ");
            if (char.TryParse(Console.ReadLine(), out char choice))
            {
                switch (choice)
                {
                    case 'y':
                    case 'Y':
                        if (text == "MembershipMenu")
                        {
                            MembershipMenu();
                        }
                        else if (text == "Logout")
                        {
                            accountLoggedIn = null;
                            MainMenu();
                        }
                        break;
                    case 'n':
                    case 'N':
                        ClearCurrentConsoleLine();
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        return false;
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
            ClearCurrentConsoleLine();
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
    }

    public static string GetStringLoginForm(string text)
    {
        while (true)
        {
            Console.Write($"[{text}]: ");
            string value = Console.ReadLine();

            // Check Q : quit
            if (value == "B" || value == "b")
            {
                if (CheckChooseBack("MembershipMenu") == false)
                {
                    ClearCurrentConsoleLine();
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    GetStringLoginForm(text);
                }
            }

            if (string.IsNullOrEmpty(value))
            {
                Console.Write($"Please Enter {text} ");
                Console.ReadKey();
                ClearCurrentConsoleLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
            else
            {
                return value;
            }
        }
    }

    public static string GetStringRegisterForm(string text)
    {
        while (true)
        {
            Console.Write($"Enter {text}: ");
            string value = Console.ReadLine();

            // Check Q : quit
            if (value == "B" || value == "b")
            {
                if (CheckChooseBack("MembershipMenu") == false)
                {
                    ClearCurrentConsoleLine();
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    GetStringRegisterForm(text);
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
                        Console.Write("Please enter username that is at least 3 characters long and uses only a-z, A-Z, 0-9 or _ characters");
                        Console.ReadKey();
                    }
                    else
                    {
                        AccountBLL accountBLL = new AccountBLL();
                        Account account = accountBLL.SearchByUsername(value);
                        if (account != null)
                        {
                            isValid = false;
                            Console.Write("Username already exist! ");
                            Console.ReadKey();
                        }
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
                    else
                    {
                        AccountBLL accountBLL = new AccountBLL();
                        Account account = accountBLL.SearchByEmail(value);
                        if (account != null)
                        {
                            isValid = false;
                            Console.Write("Email already exist! ");
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
                ClearCurrentConsoleLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
        }
    }

    public static void ClearCurrentConsoleLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);
    }

    public static string ModifyString(string value)
    {
        return string.Join(" ", value.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries));
    }

    public static void RechargeMenu()
    {
        while (true)
        {
            Console.Clear();
            var table = new Table();
            table.AddColumn(new TableColumn(new Text("Game Market Application\nGroup 2 - PF1122 Version 0.1\nRecharge Menu").Centered()));
            table.AddRow("1. Add 5$");
            table.AddRow("2. Add 10$");
            table.AddRow("3. Add 20$");
            table.AddRow("4. Add 30$");
            table.AddRow("5. Add 60$");
            table.AddRow("0. Back");
            AnsiConsole.Write(table);
            Console.Write("Your Choice: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {

                    case 1:
                        AddFunds(choice);
                        break;
                    case 2:

                        AddFunds(choice);
                        break;

                    case 3:

                        AddFunds(choice);
                        break;

                    case 4:

                        AddFunds(choice); ;
                        break;

                    case 5:

                        AddFunds(choice);
                        break;

                    case 0:
                        AccountMenu();
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

    public static void AddFunds(int choice)
    {
        string password = GetStringLoginForm("Password");
        if (password == accountLoggedIn.Password)
        {
            switch (choice)
            {
                case 1:
                    accountLoggedIn.Money += 5;
                    break;
                case 2:
                    accountLoggedIn.Money += 10;
                    break;
                case 3:
                    accountLoggedIn.Money += 20;
                    break;
                case 4:
                    accountLoggedIn.Money += 30;
                    break;
                case 5:
                    accountLoggedIn.Money += 60;
                    break;

            }
            Console.Write("Add Funds successful! ");
            Console.ReadKey();
        }
        else
        {
            Console.Write("Password incorrect! ");
            Console.ReadKey();
        }

    }

    public static void ViewProfileMenu()
    {
        while (true)
        {
            Console.Clear();
            var table = new Table();
            table.AddColumn(new TableColumn(new Text("Game Market Application\nGroup 2 - PF1122 Version 0.1\nUser Profile (B: back)").Centered()));
            table.AddRow($"Name: {accountLoggedIn.Username}\n");
            table.AddRow($"Real Name: {accountLoggedIn.Realname}\n");
            table.AddRow($"Address: {accountLoggedIn.Address}\n");
            table.AddRow($"Email: {accountLoggedIn.Email}\n");
            table.AddRow($"Money: {accountLoggedIn.Money} $\n");
            table.AddRow($"Create Date: {accountLoggedIn.CreateDate}\n");
            table.AddRow("0. Back");
            AnsiConsole.Write(table);
            Console.Write("Your Choice: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {

                    case 1:
                        AddFunds(choice);
                        break;
                    case 2:

                        AddFunds(choice);
                        break;

                    case 3:

                        AddFunds(choice);
                        break;

                    case 4:

                        AddFunds(choice); ;
                        break;

                    case 5:

                        AddFunds(choice);
                        break;

                    case 0:
                        AccountMenu();
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



