using Microsoft.Data.Sqlite;

namespace CsvLoadTester;

public static class SqlExecutor
{
    public static async Task<List<string>> GetAllTableList(SqliteConnection connection)
    {
        var query = @"SELECT name FROM sqlite_master WHERE type IN ('table', 'view') AND name NOT LIKE 'sqlite_%' UNION ALL SELECT name FROM sqlite_temp_master WHERE type IN ('table', 'view') ORDER BY 1;";

        await using var cmd = new SqliteCommand(query, connection);

        var list = new List<string>();
        try
        {
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var tableName = reader.GetString(0);
                list.Add(tableName);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return list;
    }
}
