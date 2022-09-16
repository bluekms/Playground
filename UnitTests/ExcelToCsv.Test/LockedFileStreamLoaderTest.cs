using System.Reflection;

namespace ExcelToCsv.Test;

public class LockedFileStreamLoaderTest
{
    private const string relativeFileName = @"..\..\..\..\..\..\StaticData\SampleStaticData.xlsx";
    private readonly string excelFileName;

    public LockedFileStreamLoaderTest()
    {
        var solutionPath = Assembly.GetEntryAssembly().GetName().Name;
        excelFileName = Path.Combine(solutionPath, relativeFileName);
    }

    [Fact]
    public void ExcelFileOpen()
    {
        var loader = new LockedFileStreamLoader(excelFileName);
        Assert.NotNull(loader);
    }

    [Fact]
    public void AlreadyOpenExcelFile()
    {
        var stream = File.Open(excelFileName, FileMode.Open, FileAccess.Read);
        Assert.NotNull(stream);
        
        var loader = new LockedFileStreamLoader(excelFileName);
        Assert.NotNull(loader);
        
        Assert.True(loader.IsTemp);
    }

    [Fact]
    public void DeleteTempFile()
    {
        var stream = File.Open(excelFileName, FileMode.Open, FileAccess.Read);
        var tempFileName = string.Empty;
        using (var loader = new LockedFileStreamLoader(excelFileName))
        {
            tempFileName = loader.TempFileName;
            Assert.True(loader.IsTemp);
        }

        var exists = File.Exists(tempFileName);
        Assert.False(exists);
    }
}