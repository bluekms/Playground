using System.Runtime.CompilerServices;

namespace StaticDataLibrary.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class OrderAttribute : Attribute
{
    public int Order { get; }

    public OrderAttribute([CallerLineNumber] int order = 0)
    {
        Order = order;
    }
}
