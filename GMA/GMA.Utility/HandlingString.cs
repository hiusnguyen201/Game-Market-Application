using System.Globalization;

namespace GMA.Utility;

public class HandlingString
{
    public static Func<string, string> ModifyString = (value) => string.Join(" ", value.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries));

    public static Func<double, string> FormatCurrencyVND = (money) => money.ToString("C", CultureInfo.GetCultureInfo("vi-VN")).Replace(CultureInfo.GetCultureInfo("vi-VN").NumberFormat.CurrencySymbol, "VND");
    
    public static Func<string, string> FormatSpecialName = (text) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
    
    public static string GetPassword()
    {
        string password = "";
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);

            if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");
            }
        }
        while (key.Key != ConsoleKey.Enter);
        Console.WriteLine();
        return password;
    }
}