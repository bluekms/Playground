namespace ExcelToCsv.Test;

public class CellLocationTest
{
    [Theory]
    [InlineData("a1")]
    [InlineData("A1")]
    [InlineData("Bbb1234")]
    [InlineData("xfd1048576")]
    [InlineData("XFD1048576")]
    public void NormalCellNameTest(string cellName)
    {
        var cellLocation = new CellLocation(cellName);
        Assert.NotNull(cellLocation);
    }

    [Theory]
    [InlineData("ㄱ1")]
    [InlineData("a-100")]
    [InlineData("A!1")]
    [InlineData("12Bbb1234")]
    [InlineData("Bbb-1234")]
    [InlineData("[xfd]1048576")]
    [InlineData("XFD1048576789")]
    public void InvalidCellNameTest(string cellName)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CellLocation(cellName));
    }
    
    [Theory]
    [InlineData("XFE1048576")]
    [InlineData("ZZZ1048576")]
    public void InvalidColumnNameTest(string cellName)
    {
        Assert.Throws<ColumnNameOutOfRangeException>(() => new CellLocation(cellName));
    }
    
    [Theory]
    [InlineData("A0")]
    [InlineData("xfd1048577")]
    public void InvalidRowNumberTest(string cellName)
    {
        Assert.Throws<RowNameOutOfRangeException>(() => new CellLocation(cellName));
    }
}