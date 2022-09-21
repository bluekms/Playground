using System.Text.Json;
using CommandLine;

namespace ExcelToCsv;

public sealed class ColumnSchema
{
    public ColumnSchema(int columIndex, string? sourceSchema)
    {
        ColumIndex = columIndex;

        if (sourceSchema != null)
        {
            SetSchema(sourceSchema);
        }
    }

    public int ColumIndex { get; }
    public string? SourceSchema { get; private set; }
    public SchemaData? SchemaData { get; private set; }
    public string? SourceName { get; private set; }

    public override string ToString()
    {
        var json = JsonSerializer.Serialize(SchemaData);
        return $"[{SchemaData?.Name ?? "null"}]\t{json}";
    }

    public void SetSourceName(string sourceName)
    {
        SourceName = sourceName;
        SchemaData?.SetName(sourceName);
    }

    public void SetSchema(string sourceSchema)
    {
        SourceSchema = sourceSchema;
        CreateSchemaData(sourceSchema);
    }

    private void CreateSchemaData(string sourceSchema)
    {
        Parser.Default.ParseArguments<SchemaOptions>(sourceSchema?.Split(' '))
            .WithParsed(RunOptions)
            .WithNotParsed(HandleParseError);   
    }

    private void RunOptions(SchemaOptions options)
    {
        SchemaData = new SchemaData(options);
    }

    private static void HandleParseError(IEnumerable<Error> errors)
    {
        Console.WriteLine($"Error {errors.Count()}");
        foreach (var error in errors)
        {
            Console.WriteLine(error.ToString());
        }
    }
}