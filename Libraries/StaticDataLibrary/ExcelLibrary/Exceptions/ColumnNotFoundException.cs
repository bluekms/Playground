namespace StaticDataLibrary.ExcelLibrary.Exceptions;

public class ColumnNotFoundException : Exception
{
    public ColumnNotFoundException(string columnName)
        : base($"Column not found: {columnName}")
    {
    }
}
