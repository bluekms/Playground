using System.Data;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StaticDataLibrary.RecordLibrary;

namespace StaticDataLibrary.ValidationLibrary;

public static class ForeignChecker
{
    public static async Task CheckAsync<T>(SqliteConnection connection, List<string>? errors = null) where T : DbContext
    {
        var tableInfoList = TableFinder.FindAllTablesWithForeignKey<T>();
        foreach (var tableInfo in tableInfoList)
        {
            if (tableInfo.ForeignInfoList == null)
            {
                throw new NullReferenceException($"{nameof(tableInfo.ForeignInfoList)}");
            }
            
            foreach (var foreignInfo in tableInfo.ForeignInfoList)
            {
                var results = await CheckForeignKeyAsync(connection, tableInfo, foreignInfo);
                if (results.Count == 0)
                {
                    continue;
                }
                
                var sb = new StringBuilder();
                foreach (var result in results)
                {
                    sb.AppendLine($"{foreignInfo.CurrentTableName}.{foreignInfo.CurrentColumnName} 에서 사용된 {result.ToString()}");
                }

                if (errors is null)
                {
                    throw new DataException(sb.ToString());
                }
                else
                {
                    errors.Add(sb.ToString());
                }
            }
        }
    }
    
    public sealed record NotExistsForeignKeyResult(string ForeignTableName, string ColumnName, string? expected)
    {
        public override string ToString()
        {
            return $"'{expected}'는 {ForeignTableName}.{ColumnName} 에 존재하지 않습니다.";
        }
    }
    
    public static async Task<List<NotExistsForeignKeyResult>> CheckForeignKeyAsync(SqliteConnection connection, TableInfo tableInfo, TableInfo.ForeignInfo foreignInfo)
    {
        var sqlExpected = RecordQueryBuilder.SelectForeignKeyListQuery(tableInfo, foreignInfo);
                
        await using var cmdExpected = new SqliteCommand(sqlExpected, connection);

        var expectedList = new List<object>();
        try
        {
            await using var readerExpected = await cmdExpected.ExecuteReaderAsync();
            while (await readerExpected.ReadAsync())
            {
                var id = readerExpected.GetValue(0);
                expectedList.Add(id);
            }
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"{foreignInfo.ForeignTableName}.{foreignInfo.ForeignColumnName} sql: {sqlExpected}", e);
        }
        
        var list = new List<NotExistsForeignKeyResult>();
        foreach (var expected in expectedList)
        {
            if (expected is DBNull)
            {
                continue;
            }
            
            var sqlResult = RecordQueryBuilder.SelectCountQuery(foreignInfo, expected);
                    
            await using var cmdResult = new SqliteCommand(sqlResult, connection);

            try
            {
                await using var readerResult = await cmdResult.ExecuteReaderAsync();
                while (await readerResult.ReadAsync())
                {
                    var count = readerResult.GetInt32(0);
                    if (count <= 0)
                    {
                        list.Add(new(foreignInfo.ForeignTableName, foreignInfo.ForeignColumnName, expected?.ToString()));
                    }
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"{e.Message}.\n{foreignInfo.ForeignTableName}.{foreignInfo.ForeignColumnName} sql: {sqlResult}", e);
            }
        }

        return list;
    }
}