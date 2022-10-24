using System.Collections;
using Microsoft.Data.Sqlite;

namespace StaticDataLibrary.RecordLibrary;

public static class RecordSqlExecutor
{
    public static async Task InsertAsync(SqliteConnection connection, TableInfo tableInfo, IList dataList,
        SqliteTransaction transaction)
    {
        var query = RecordQueryBuilder.InsertQuery(tableInfo, out var parameters);
        
        foreach (var data in dataList)
        {
            await using var command = new SqliteCommand(query, connection, transaction);
            
            foreach (var name in parameters)
            {
                var value = data.GetType()
                    .GetProperty(name)!
                    .GetValue(data, null) ?? DBNull.Value;

                command.Parameters.Add(new($"@{name}", value));
            }
            
            await command.ExecuteNonQueryAsync();
        }
    }

    public sealed record NotExistsForeignKeyResult(string ForeignTableName, string ColumnName, string expected)
    {
        public override string ToString()
        {
            return $"{ForeignTableName}.{ColumnName}에 {expected} 값이 없습니다.";
        }
    }

    public static async Task<List<NotExistsForeignKeyResult>> CheckForeignKey(SqliteConnection connection, TableInfo tableInfo, TableInfo.ForeignInfo foreignInfo)
    {
        var sqlExpected = RecordQueryBuilder.SelectForeignKeyListQuery(tableInfo, foreignInfo);
                
        await using var cmdExpected = new SqliteCommand(sqlExpected, connection);
        await using var readerExpected = await cmdExpected.ExecuteReaderAsync();
                
        var expectedList = new List<object>();
        while (await readerExpected.ReadAsync())
        {
            var id = readerExpected.GetValue(0);
            expectedList.Add(id);
        }
        
        var list = new List<NotExistsForeignKeyResult>();
        foreach (var expected in expectedList)
        {
            var sqlResult = RecordQueryBuilder.SelectCountQuery(foreignInfo, expected);
                    
            await using var cmdResult = new SqliteCommand(sqlResult, connection);
            await using var readerResult = await cmdResult.ExecuteReaderAsync();
            while (await readerResult.ReadAsync())
            {
                var count = readerResult.GetInt32(0);
                if (count <= 0)
                {
                    list.Add(new(foreignInfo.ForeignTableName, foreignInfo.ColumnName, expected.ToString()));
                }
            }
        }

        return list;
    }
}