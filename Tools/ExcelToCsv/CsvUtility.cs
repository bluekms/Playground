namespace ExcelToCsv;

public static class CsvUtility
{
    public static string? ToCsv(string? str)
    {
        if (str == null)
        {
            return null;
        }

        var csv = str;
        
        if (csv.Contains(','))
        {
            csv = $"\"{csv}\"";
        }

        return csv;
    }

    public static string? ToNormal(string? csv)
    {
        if (csv == null)
        {
            return null;
        }

        var str = csv;

        if (str.Contains(','))
        {
            str = str.Substring(1, str.Length - 2);
        }

        return str;
    }
}