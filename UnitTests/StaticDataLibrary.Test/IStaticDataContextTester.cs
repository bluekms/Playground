namespace StaticDataLibrary.Test;

public interface IStaticDataContextTester
{
    public void RequiredAttributeTest();
    public void MustUseSealedClass();
    public Task LoadCsvToRecordTestAsync();
    public Task RangeAttributeTestAsync();
    public Task InsertSqliteTestAsync();
    public Task ForeignTableTestAsync();
}