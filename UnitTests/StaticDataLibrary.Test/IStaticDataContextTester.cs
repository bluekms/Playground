namespace StaticDataLibrary.Test;

public interface IStaticDataContextTester
{
    public void RequiredAttributeTest();
    public Task LoadCsvToRecordTestAsync();
    public Task RangeAttributeTestAsync();
    public Task InsertSqliteTestAsync();
}