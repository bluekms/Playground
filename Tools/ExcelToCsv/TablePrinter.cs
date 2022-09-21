using System.Net.Sockets;
using System.Text;

namespace ExcelToCsv;

public sealed class TablePrinter
{
    private const string Extension = ".csv";
    private const char Separator = ',';
    
    private readonly TableLoader table;
    private readonly HashSet<string> targets;

    public TablePrinter(TableLoader table)
    {
        this.table = table;
        targets = table.GetColumnTargets();
        
        if (targets.Count == 0)
        {
            targets.Add(SchemaData.TargetAll);
        }
    }

    public string? OutputFileName { get; private set; }

    public async Task DoPrintAsync(string outputPath, string? target = null)
    {
        if (string.IsNullOrWhiteSpace(target))
        {
            await AllTargetPrintAsync(outputPath);
        }
        else
        {
            await TargetPrintAsync(outputPath, target);
        }
    }

    private async Task AllTargetPrintAsync(string outputPath)
    {
        foreach (var target in targets)
        {
            try
            {
                await TargetPrintAsync(outputPath, target);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    private async Task TargetPrintAsync(string outputPath, string target)
    {
        if (!targets.Contains(target))
        {
            var targetStr = string.Join(Separator, targets.ToArray());
            throw new InvalidDataException($"{table.Name}에는 {target}이 없습니다. 입력된 Target: {targetStr}");
        }
        
        OutputFileName = SetOutputFileName(outputPath, target);

        var rows = table.GetOutputData(target);
        
        await using var writer = new StreamWriter(OutputFileName, false, Encoding.UTF8);
        for (var i = 0; i < rows.Count; ++i)
        {
            var line = string.Join(Separator, rows[i]);

            if (i == rows.Count - 1)
            {
                await writer.WriteAsync(line);
            }
            else
            {
                await writer.WriteLineAsync(line);
            }
        }
        writer.Close();
    }

    private string SetOutputFileName(string outputPath, string target)
    {
        var path = target.Equals(SchemaData.TargetAll)
            ? outputPath
            : Path.Combine(outputPath, target);

        var di = new DirectoryInfo(path);
        if (!di.Exists)
        {
            di.Create();
        }

        return Path.Combine(path, $"{table.Name}{Extension}");
    }
}