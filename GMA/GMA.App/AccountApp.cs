using GMA.BLL;
using GMA.Models;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace GMA.App;

public class AccountApp
{
    public static string patternUsername = "^[^\\s][a-zA-Z0-9_-]{3,}$";
    public static string patternRealName = "^[A-Za-z ]{2,}$";
    public static string patternEmail = @"^[^\\s][a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    public static Account accountLoggedIn = null;

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
        table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 Version : 0.1\nLogin Form (B: back)").Centered()));
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

    public static void RegisterForm()
    {
        Console.Clear();
        var table = new Table();
        table.Width = 45;
        table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 Version : 0.1\nRegister Form (B: back)").Centered()));
        AnsiConsole.Write(table);

        string username = GetStringRegisterForm("Username");

        string password = GetStringRegisterForm("Password");

        string realname = MainMenuApp.ModifyString(GetStringRegisterForm("Real Name"));

        string email = GetStringRegisterForm("Email");

        string address = MainMenuApp.ModifyString(GetStringRegisterForm("Address"));

        AccountBLL accountBLL = new AccountBLL();
        int result = accountBLL.Save(new Account(username, password, realname, email, address));
        if (result != 0)
        {
            Console.Write("New Account Created Successfully! ");
            Console.ReadKey();
            Console.WriteLine();
        }

        MainMenuApp.CheckContinue("CreateAccount");
    }

    public static void AccountMenu()
    {
        while (true)
        {
            Console.Clear();
            var table = new Table();
            table.Width = 45;
            table.AddColumn(new TableColumn(new Text($"[Game Market Application]\nGroup 2 - PF1122 Version : 0.1\nUsername: {accountLoggedIn.Username} | Money: {accountLoggedIn.Money} $").Centered()));
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
        table.AddRow($"\nName: {accountLoggedIn.Username}\n").AddRow(new Rule());
        table.AddRow($"\nReal Name: {accountLoggedIn.Realname}\n").AddRow(new Rule());
        table.AddRow($"\nEmail: {accountLoggedIn.Email}\n").AddRow(new Rule());
        table.AddRow($"\nAddress: {accountLoggedIn.Address}\n").AddRow(new Rule());
        table.AddRow($"\nMoney: {accountLoggedIn.Money} $\n").AddRow(new Rule());
        table.AddRow($"\nCreate Date: {accountLoggedIn.CreateDate.ToString("dd/MM/yyyy")}\n");
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
                        CheckPassToAddFunds(choice);
                        break;
                    case 2:

                        CheckPassToAddFunds(choice);
                        break;

                    case 3:

                        CheckPassToAddFunds(choice);
                        break;

                    case 4:

                        CheckPassToAddFunds(choice); ;
                        break;

                    case 5:

                        CheckPassToAddFunds(choice);
                        break;

                    case 0:
                        AccountMenu();
                        break;

                    default:
                        Console.Write("Your choice is not exist! ");
                        Console.ReadKey();
                        break;
                }
                AccountMenu();
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
            AccountBLL accountBLL = new AccountBLL();
            accountBLL.UpdateMoney(accountLoggedIn.AccountId, accountLoggedIn.Money);
            Console.Write("Add Funds successful! ");
            Console.ReadKey();
        }
        else
        {
            Console.Write("Password incorrect! ");
            Console.ReadKey();
        }

    }

   public static string GetStringLoginForm(string text)
    {
        while (true)
        {
            string value;
            if(text == "Password")
            {
                Console.Write($"[{text}]: ");
                value = GetPassword();
            }
            else
            {
                Console.Write($"[{text}]: ");
                value = Console.ReadLine();
            }

            // Check Q : quit
            if (value == "B" || value == "b")
            {
                if (MainMenuApp.CheckChooseBack("MembershipMenu") == false)
                {
                    MainMenuApp.ClearCurrentConsoleLine();
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    GetStringLoginForm(text);
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

    public static string GetStringRegisterForm(string text)
    {
        while (true)
        {
            string value;
            if(text == "Password")
            {
                Console.Write($"- Enter {text}: ");
                value = GetPassword();
            }
            else
            {
                Console.Write($"- Enter {text}: ");
                value = Console.ReadLine();
            }

            // Check Q : quit
            if (value == "B" || value == "b")
            {
                if (MainMenuApp.CheckChooseBack("MembershipMenu") == false)
                {
                    MainMenuApp.ClearCurrentConsoleLine();
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
                        Console.Write("Please enter username that is at least 3 characters long and uses only a-z, A-Z, 0-9, _ characters");
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
                else if (text == "Real Name")
                {
                    if(!Regex.IsMatch(value, patternRealName))
                    {
                        isValid = false;
                        Console.Write("Please enter real name that is at least 2 characters long and uses only a-z, A-Z, whitespace characters");
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
}
