using System.Reflection;
using StaticDataLibrary;
using StaticDataLibrary.Attributes;

namespace ExcelLibrary;

public class RecordReaderFromLibrary
{
    public RecordReaderFromLibrary(string sheetName)
    {
        var record = GetTargetRecord(sheetName);
        if (record == null)
        {
            throw new FileNotFoundException($"{sheetName}을 로드할 수 있는 record가 없습니다.");
        }

        var columnInfoList = SerializeAllColumns(record);
        foreach (var ci in columnInfoList)
        {
            Console.WriteLine($"  --- {ci}");    
        }
    }

    private Type? GetTargetRecord(string sheetName)
    {
        var records = Assembly.GetAssembly(typeof(AssemblyEntry))!
            .GetTypes()
            .Where(x => x.Namespace is ConstData.RecordsNamespace)
            .Where(x => x.IsSealed)
            .Where(x => x.IsClass)
            .Where(x => x.GetProperties().Length > 0)
            .ToList();

        var target = records.SingleOrDefault(x => x.Name == sheetName);
        if (target != null)
        {
            return target;
        }
        
        foreach (var record in records)
        {
            var sheetNameAttribute = record.GetCustomAttributes()
                .SingleOrDefault(x => x.GetType() == typeof(SheetName));
            
            if (sheetNameAttribute == null)
            {
                continue;
            }

            if ((sheetNameAttribute as SheetName)!.Name.Equals(sheetName))
            {
                return record;
            }
        }

        return null;
    }

    private List<string> SerializeAllColumns(Type t)
    {
        var columns = new List<string>();
        Traverse(t, t.Name, columns);
        return columns;
    }

    private void Traverse(Type t, string name, List<string> columns)
    {
        var isTerminal = ConstData.TerminalTypes.Contains(t) || t.IsEnum;

        var isTerminalList = t.GetGenericArguments().Length == 1
                             && ConstData.TerminalTypes.Contains(t.GetGenericArguments()[0]);

        if (isTerminal || isTerminalList)
        {
            columns.Add(name);
        }
        else
        {
            var isGenericList = t.IsGenericType
                                && (t.GetGenericTypeDefinition() == typeof(List<>) ||
                                    t.GetGenericTypeDefinition() == typeof(HashSet<>));
            if (isGenericList)
            {
                var innerType = t.GetGenericArguments()[0];
                var innerName = t.GetGenericArguments()[0].Name;
                Traverse(innerType, innerName, columns);
            }
            else
            {
                var properties = t.GetProperties();
                foreach (var pi in properties)
                {
                    var columnNameAttribute = pi.GetCustomAttributes()
                        .SingleOrDefault(x => x.GetType() == typeof(ColumnName));

                    var columnName = pi.Name;
                    if(columnNameAttribute != null)
                    {
                        columnName = (columnNameAttribute as ColumnName)!.Name;
                    }
                    Traverse(pi.PropertyType, columnName, columns);
                }
            }
        }
    }
}