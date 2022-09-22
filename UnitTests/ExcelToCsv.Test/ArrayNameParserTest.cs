namespace ExcelToCsv.Test;

public class ArrayNameParserTest
{
    [Theory]
    [InlineData("Amount[0]", true, "Amount", 0)]
    [InlineData("Probability[80]", true, "Probability", 80)]
    [InlineData("Amount[-99]", false, null, null)]
    [InlineData("Id[]", false, null, null)]
    [InlineData("12Id[3]", false, null, null)]
    [InlineData("_Id[3]", false, null, null)]
    [InlineData("-Id[3]", false, null, null)]
    [InlineData("ScenarioEventId", false, null, null)]
    public void SuccessTest(string columnName, bool isArrayName, string? name, int? index)
    {
        var parser = new ContainerNameParser(columnName);
        
        Assert.Equal(parser.IsContainerItem, isArrayName);
        Assert.Equal(parser.PureName, name);
        Assert.Equal(parser.ContainerIndex, index);
    }
}