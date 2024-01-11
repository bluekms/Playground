namespace StaticDataLibrary.ExcelLibrary.Exceptions;

public class A1CellException : Exception
{
    public A1CellException(string a1Value, Exception? innerException = null)
        : base($"Cell A1 should contain the starting position of the data header. Current value: {a1Value}", innerException)
    {
    }
}
