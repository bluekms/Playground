using StaticDataLibrary.ExcelLibrary;

namespace ExcelToCsv.Test;

public sealed class LockedFileStreamLoaderTest
{
    private readonly string excelFileName;

    public LockedFileStreamLoaderTest()
    {
        excelFileName = Path.Join(AppContext.BaseDirectory, @"../../../../..", @"StaticData/SampleStaticData.xlsx");
    }

    [FactWithoutEnvironmentVariable("CI")]
    public void ExcelFileOpen()
    {
        var loader = new LockedFileStreamLoader(excelFileName);
        Assert.NotNull(loader);
    }

    [FactWithoutEnvironmentVariable("CI")]
    public void AlreadyOpenExcelFile()
    {
        var stream = File.Open(excelFileName, FileMode.Open, FileAccess.Read);
        Assert.NotNull(stream);
        
        var loader = new LockedFileStreamLoader(excelFileName);
        Assert.NotNull(loader);
        
        Assert.True(loader.IsTemp);
    }

    [FactWithoutEnvironmentVariable("CI")]
    public void DeleteTempFile()
    {
        File.Open(excelFileName, FileMode.Open, FileAccess.Read);
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