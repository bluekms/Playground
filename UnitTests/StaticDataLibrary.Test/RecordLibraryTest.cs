using StaticDataLibrary.RecordLibrary;

namespace StaticDataLibrary.Test;

public sealed class RecordLibraryTest
{
    private const int TestTableCount = 5;
    private const string TestStaticDataPath = @"../../../../../StaticData/__TestStaticData";

    [Fact]
    public void TableCountTest()
    {
        var tableInfoList = TableFinder.Find<TestStaticDataContext>();
        Assert.True(tableInfoList.Count == TestTableCount, 
            $"Check the suffix. {TableInfo.DbSetNameSuffix}");
    }

    [Fact]
    public void RequiredAttributeTest()
    {
        StaticDataContextTester<TestStaticDataContext>.RequiredAttributeTest();
    }

    [Fact]
    public async Task LoadCsvTest()
    {
        await StaticDataContextTester<TestStaticDataContext>.LoadCsvTest(TestStaticDataPath);
    }

    [Fact]
    public async Task RangeAttributeTest()
    {
        await StaticDataContextTester<TestStaticDataContext>.RangeAttributeTest(TestStaticDataPath);
    }
    
    [Theory]
    [InlineData("TargetTestTable", 5, "INSERT INTO TargetTestTable VALUES (104,9,9);")]
    [InlineData("NameTestTable", 5, "INSERT INTO NameTestTable VALUES (104,10,10);")]
    [InlineData("ArrayTestTable", 5, "INSERT INTO ArrayTestTable VALUES (104,9,10,19,90,,);")]
    [InlineData("ClassListTestTable", 3, "INSERT INTO ClassListTestTable VALUES (20220003,CCC,영어,D,,,,,\"국어, 수학 미응시\");")]
    [InlineData("ComplexTestTable", 2, "INSERT INTO ComplexTestTable VALUES (1학년2반,20220201,XXX,국어,C,영어,C,수학,C,,20220202,YYY,국어,A,수학,A,,,영어 미응시,20220203,,,,,,,,,담임 미정);")]
    public async void RecordQueryBuilderTest(string dbSetName, int rowCount, string expected)
    {
        var tableInfoList = TableFinder
            .Find<TestStaticDataContext>()
            .Single(x => x.DbSetName == dbSetName);
        
        var fileName = Path.Combine(
            Directory.GetCurrentDirectory(),
            TestStaticDataPath,
            $"{tableInfoList.SheetName}.csv");
        
        var query = RecordQueryBuilder.InsertQuery(tableInfoList.RecordType, tableInfoList.DbSetName, out var parameters);

        // 대체로 가장 마지막 데이터가 가장 독특한 형태
        var dataList = await RecordParser.GetDataList(tableInfoList, fileName);
        var lastData = dataList[^1]!;
        
        var propertiesCount = lastData.GetType().GetProperties().Length;
        var parametersCount = parameters?.Count ?? 0;
        Assert.Equal(propertiesCount, parametersCount);
        
        foreach (var name in parameters!)
        {
            var value = lastData.GetType()
                .GetProperty(name)!
                .GetValue(lastData, null) ?? DBNull.Value;

            query = query.Replace($"@{name}", value.ToString());
        }
        
        Assert.Equal(rowCount, dataList.Count);
        Assert.Equal(query, expected);
    }
}