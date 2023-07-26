namespace GMA.App;
using Spectre.Console; // Test
public class Menu
{
    public static bool isLoged = false;
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
            table.AddRow("1. Membership and Profile");
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

    public static void LoginForm()
    {
        while (true)
        {
            Console.Clear();
            var table = new Table();
            table.AddColumn(new TableColumn(new Text("Game Market Application\nGroup 2 - PF1122 Version 0.1\nLogin Form").Centered()));
            AnsiConsole.Write(table);
            Console.Write("[Username] (Q: quit): ");
            string username = Console.ReadLine();
            if (String.IsNullOrEmpty(username))
            {
                Console.Write("Username cannot be null! ");
                Console.ReadKey();
                // ClearCurrentConsoleLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
            else if (username == "Q" || username == "q")
            {
                MembershipMenu();
            }
            Console.Write("[Password] (Q: quit): ");
            string password = Console.ReadLine();
            if (String.IsNullOrEmpty(password))
            {
                Console.Write("Password cannot be null! ");
                Console.ReadKey();
                // ClearCurrentConsoleLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
            else if (password == "Q" || password == "q")
            {
                MembershipMenu();
            }


        }
    }
    public static void AccountMenu()
    {
        while (true)
        {
            Console.Clear();
            var table = new Table();
            table.AddColumn(new TableColumn(new Text("Game Market Application\nGroup 2 - PF1122 Version 0.1\nAccount Name").Centered()));
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
                        break;

                    case 2:
                        break;

                    case 3:
                        break;

                    case 4:
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
        while (true)
        {
            Console.Clear();
            var table = new Table();
            table.AddColumn(new TableColumn(new Text("Game Market Application\nGroup 2 - PF1122 Version 0.1\nRegister Form (B: back)").Centered()));
            AnsiConsole.Write(table);

            Console.Write("Enter Username: ");
            string username = GetStringForm("Username");
            
            Console.WriteLine("Enter Password: ");
            string password = GetStringForm("Password");
            
            Console.WriteLine("Confirm Password: ");
            string confirmPassword = GetStringForm("Confirm Password");
            
            Console.WriteLine("Enter Realname: ");
            string realname = GetStringForm("Real Name");
            
            Console.WriteLine("Enter Phone: ");
            string phone = GetStringForm("Phone");

            Console.WriteLine("Enter Email: ");
            string email = GetStringForm("Email");

            Console.WriteLine("Enter Address: ");
            string address = GetStringForm("Address");
        }
    }

    public static string GetStringForm(string text)
    {
        while(true)
        {
            Console.Write($"Enter {text}: ");
            string value = ModifyString(Console.ReadLine());
            if (String.IsNullOrEmpty(text))
            {
                Console.Write($"{text} cannot be null! ");
                Console.ReadKey();
                // ClearCurrentConsoleLine();
                // Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
            else if(username == "Q"||username == "q")
            {
                MembershipMenu();
            }
            else
            {

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
}



