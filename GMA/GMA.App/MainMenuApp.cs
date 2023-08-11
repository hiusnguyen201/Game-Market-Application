﻿namespace GMA.App;

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
            table.AddColumn(new TableColumn(new Text("[Game Market Application]\nGroup 2 - PF1122 Version : 0.1\nWelcome to Game Market").Centered()));
            table.AddRow("1. Manage Membership");
            table.AddRow("2. View Store");
            table.AddRow("3. View Cart");
            table.AddRow("0. Exit");
            AnsiConsole.Write(table);
            Console.Write("Your Choice: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        AccountApp.MembershipMenu();
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

    public static bool CheckChooseBack(string text)
    {
        while (true)
        {
            Console.Write("Are you sure (Y/N): ");
            if (char.TryParse(Console.ReadLine(), out char choice))
            {
                switch (char.ToUpper(choice))
                {
                    case 'Y':
                        if (text == "MembershipMenu")
                        {
                            AccountApp.MembershipMenu();
                        }
                        else if (text == "Logout")
                        {
                            AccountApp.accountLoggedIn = null;
                            MainMenu();
                        }
                        break;
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
                ClearCurrentConsoleLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write("Invalid choice! Try again ");
                Console.ReadKey();
            }
            ClearCurrentConsoleLine();
            Console.SetCursorPosition(0, Console.CursorTop - 1);
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



