using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace ExcelToCsv;

public sealed class CellLocationParser
{
    private const string CellNamePattern = @"^([a-zA-Z]{1,3})(\d{1,7})\b";
    private const string MinColumnName = "A";
    private const string MaxColumnName = "XFD";
    private const int MinColumnNumber = 0;
    private const int MaxColumnNumber = 16384;  // XFD
    private const int MinRowNumber = 1;
    private const int MaxRowNumber = 1048576;

    public int ColumnNumber { get; }
    public int RowNumber { get; }
    
    public CellLocationParser(string cellName)
    {
        var reg = new Regex(CellNamePattern);

        if (!reg.IsMatch(cellName))
        {
            throw new ArgumentOutOfRangeException($"{cellName} is not cell name.");
        }
        
        var matchCollection = reg.Matches(cellName);
        var match = matchCollection[0];
        
        var col = match.Groups[1].Value.ToUpper();
        if (string.CompareOrdinal(col, MinColumnName) < 0)
        {
            throw new ColumnNameOutOfRangeException($"{cellName} must be {col} >= {MinColumnName}.");
        }
        
        if (string.CompareOrdinal(col, MaxColumnName) > 0)
        {
            throw new ColumnNameOutOfRangeException($"{cellName} must be {col} <= {MaxColumnName}.");
        }

        ColumnNumber = GetColumnNumber(col);
        switch (ColumnNumber)
        {
            case < MinColumnNumber:
                throw new ColumnNameOutOfRangeException($"{cellName} must be {ColumnNumber} >= {MinColumnNumber}");
            case > MaxColumnNumber:
                throw new ColumnNameOutOfRangeException($"{cellName} must be {ColumnNumber} <= {MaxColumnNumber}");
        }

        RowNumber = int.Parse(match.Groups[2].Value);
        switch (RowNumber)
        {
            case < MinRowNumber:
                throw new RowNameOutOfRangeException($"{cellName} must be {RowNumber} >= {MinRowNumber}");
            case > MaxRowNumber:
                throw new RowNameOutOfRangeException($"{cellName} must be {RowNumber} <= {MaxRowNumber}");
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

public class ColumnNameOutOfRangeException : ArgumentOutOfRangeException
{
    public ColumnNameOutOfRangeException()
    {
    }

    protected ColumnNameOutOfRangeException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ColumnNameOutOfRangeException(string? paramName) : base(paramName)
    {
    }

    public ColumnNameOutOfRangeException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public ColumnNameOutOfRangeException(string? paramName, object? actualValue, string? message) : base(paramName, actualValue, message)
    {
    }

    public ColumnNameOutOfRangeException(string? paramName, string? message) : base(paramName, message)
    {
    }
}

public class RowNameOutOfRangeException : ArgumentOutOfRangeException
{
    public RowNameOutOfRangeException()
    {
    }

    protected RowNameOutOfRangeException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public RowNameOutOfRangeException(string? paramName) : base(paramName)
    {
    }

    public RowNameOutOfRangeException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public RowNameOutOfRangeException(string? paramName, object? actualValue, string? message) : base(paramName, actualValue, message)
    {
    }

    public RowNameOutOfRangeException(string? paramName, string? message) : base(paramName, message)
    {
    }
}