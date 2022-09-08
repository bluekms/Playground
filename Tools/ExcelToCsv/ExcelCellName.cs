using System.Text.RegularExpressions;

namespace ExcelToCsv;

public sealed class CellLocation
{
    private static string CellNamePattern = @"\b([A-Z]{1,3})(\d{1,7})\b";
    private static string MaxColumnName = "XFD";
    private static int MaxRowNumber = 1048576;

    public int ColumnNumber { get; init; }
    public int RowNumber { get; init; }
    
    public CellLocation(string cellName)
    {
        var reg = new Regex(CellNamePattern);

        if (!reg.IsMatch(cellName))
        {
            throw new ArgumentOutOfRangeException($"{cellName} is not cell name.");
        }
        
        var matchCollection = reg.Matches(cellName);
        var match = matchCollection[0];
        
        var col = match.Groups[1].Value;
        if (string.CompareOrdinal(col, MaxColumnName) > 0)
        {
            throw new ArgumentOutOfRangeException($"{cellName} Max column name is {MaxColumnName}.");
        }

        ColumnNumber = GetColumnNumber(col);

        RowNumber = int.Parse(match.Groups[2].Value);
        if (RowNumber > MaxRowNumber)
        {
            throw new ArgumentOutOfRangeException($"{cellName} Max row name is {MaxRowNumber}.");
        }
    }
    
    private static int GetColumnNumber(string name)
    {
        var number = 0;
        var pow = 1;
        for (var i = name.Length - 1; i >= 0; i--)
        {
            number += (name[i] - 'A' + 1) * pow;
            pow *= 26;
        }

        return number;
    }
}
