using System.Globalization;
using System.Reflection;
using StaticDataLibrary.Records;

namespace StaticDataLibrary.Test;

public sealed class CheckRecords
{
    [Fact]
    public void MustBeSealedRecord()
    {
        foreach (var t in Assembly.GetAssembly(typeof(AssemblyEntry))!.GetTypes())
        {
            if (t.Namespace!.Equals(ConstData.RecordsNamespace) == false ||
                t.GetTypeInfo().GetProperties().Length <= 0)
            {
                continue;
            }

            Assert.True(t.IsSealed, $"[{t.Name}] must be sealed record");

            var attributeList = t.GetCustomAttributes().Select(x => x.GetType().Name).ToList();
            var isRecord = attributeList.Contains("NullableContextAttribute") &&
                           attributeList.Contains("NullableAttribute");
            Assert.True(isRecord, $"[{t.Name}] must be sealed record");
        }
    }
}