using System.Reflection;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.RecordLibrary;

public sealed class RecordInfo
{
    public const string TypeNameSuffix = "Record";
    public const string DbSetNameSuffix = "Table";
    
    public Type RecordType { get; }
    public string SheetName { get; }
    public string DbSetName { get; }
    
    public RecordInfo(PropertyInfo pi)
    {
        RecordType = pi.PropertyType.GetGenericArguments().First();
        SheetName = RecordType.Name.Replace(TypeNameSuffix, string.Empty);
        DbSetName = $"{SheetName}{DbSetNameSuffix}";
        
        var sheetNameAttribute = RecordType
            .GetCustomAttributes()
            .SingleOrDefault(x => x.GetType() == typeof(SheetName));
            
        if(sheetNameAttribute != null)
        {
            SheetName = (sheetNameAttribute as SheetName)!.Name;
        }
    }
}