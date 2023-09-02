namespace GMA.App;

using Spectre.Console;

public class MainMenuApp
{
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
            table.Width = 45;
            table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 | Version : 0.1\nWelcome to Game Market").Centered()));
            table.AddRow("1. Manage Account");
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
                        if(AccountApp.accountLoggedIn != null)
                        {
                            AccountApp.AccountMenu();
                        }
                        else if(AccountApp.accountLoggedIn == null)
                        {
                            AccountApp.AuthenticationMenu();
                        }
                        break;

                    case 2:
                        GameApp.GameStoreMenu();
                        break;

                    case 3:
                        OrderApp.CartMenu();
                        break;

                    case 0:
                        Environment.Exit(1);
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

    public static bool CheckYesNo()
    {
        while (true)
        {
            Console.Write("Are You Sure (Y/N): ");
            if (char.TryParse(Console.ReadLine(), out char choice))
            {
                switch (char.ToUpper(choice))
                {
                    case 'Y':
                        return true;
                    case 'N':
                        ClearCurrentConsoleLine();
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        ClearCurrentConsoleLine();
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        ClearCurrentConsoleLine();
                        return false;
                    default:
                        Console.Write("Your Choice Is Not Exist! ");
                        Console.ReadKey();
                        ClearCurrentConsoleLine();
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        ClearCurrentConsoleLine();
                        break;
                }
            }
            else
            {
                Console.Write("Invalid Choice! Try Again! ");
                Console.ReadKey();
                ClearCurrentConsoleLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ClearCurrentConsoleLine();
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
}