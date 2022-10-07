using System.Collections;
using Microsoft.Data.Sqlite;

namespace StaticDataLibrary;

public static class RecordDataInserter
{
    public static async Task InsertAsync(string tableName, Type recordType, IList dataList, SqliteConnection connection, SqliteTransaction transaction)
    {
        var query = RecordQueryBuilder.InsertQuery(recordType, tableName, out var parameters);
        
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
}