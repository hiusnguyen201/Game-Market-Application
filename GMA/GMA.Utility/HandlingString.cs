using System.Globalization;

namespace GMA.Utility;

public class HandlingString
{
    public static Func<string, string> ModifyString = (value) => string.Join(" ", value.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries));

    public static Func<double, string> FormatCurrencyVND = (money) => money.ToString("C", CultureInfo.GetCultureInfo("vi-VN")).Replace(CultureInfo.GetCultureInfo("vi-VN").NumberFormat.CurrencySymbol, "VND");
    
    public static Func<string, string> FormatSpecialName = (text) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
}