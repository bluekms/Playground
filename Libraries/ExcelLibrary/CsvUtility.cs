namespace ExcelLibrary;

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
}