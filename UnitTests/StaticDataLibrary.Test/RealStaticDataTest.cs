namespace StaticDataLibrary.Test;

public sealed class RealStaticDataTest
{
    // TODO 경로 수정
    private const string RealStaticDataPath = @"../../../../../StaticData/__TestStaticData/Output";

    [Fact]
    public void RequiredAttributeTest()
    {
        StaticDataContextTester<TestStaticDataContext>.RequiredAttributeTest();
    }

    [Fact]
    public async Task LoadCsvTest()
    {
        await StaticDataContextTester<TestStaticDataContext>.LoadCsvTest(RealStaticDataPath);
    }

    [Fact]
    public async Task RangeAttributeTest()
    {
        await StaticDataContextTester<TestStaticDataContext>.RangeAttributeTest(RealStaticDataPath);
    }
}