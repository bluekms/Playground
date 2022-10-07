using System.Collections;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace StaticDataLibrary;

public static class RecordParser
{
    public static IList GetDataList(RecordInfo recordInfo, string fileName)
    {
        using var reader = new StreamReader(fileName, Encoding.UTF8);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
        });

        return csv.GetRecords(recordInfo.RecordType).ToList();
    }
}