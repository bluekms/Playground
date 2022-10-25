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

    public sealed record NotExistsForeignKeyResult(string ForeignTableName, string ColumnName, string? expected)
    {
        public override string ToString()
        {
            return $"'{expected}'는 {ForeignTableName}.{ColumnName} 에 존재하지 않습니다.";
        }
    }

    public static async Task<List<NotExistsForeignKeyResult>> CheckForeignKey(SqliteConnection connection, TableInfo tableInfo, TableInfo.ForeignInfo foreignInfo)
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
                throw new InvalidOperationException($"{foreignInfo.ForeignTableName}.{foreignInfo.ForeignColumnName} sql: {sqlResult}", e);
            }
        }

        return list;
    }
}