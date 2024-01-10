namespace StaticDataLibrary.ExcelLibrary;

public static class QuotationMarks
{
    public static string? Wrapped(string? str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return str;
        }

        return str.Contains(',')
            ? $"\"{str}\""
            : str;
    }

    public static string? Unwrapped(string? csv)
    {
        if (string.IsNullOrWhiteSpace(csv))
        {
            return csv;
        }

        return csv.Contains(',')
            ? csv.Substring(1, csv.Length - 2)
            : csv;
    }
}
