using System.Reflection;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.RecordLibrary;

public static class OrderedPropertySelector
{
    public static List<PropertyInfo> GetList(Type t)
    {
        return t.GetProperties()
            .Where(x => x.CanWrite)
            .Where(x => Attribute.IsDefined(x, typeof(OrderAttribute)))
            .OrderBy(x => ((OrderAttribute)x.GetCustomAttributes(typeof(OrderAttribute), false).Single()).Order)
            .ToList();
    }
}
