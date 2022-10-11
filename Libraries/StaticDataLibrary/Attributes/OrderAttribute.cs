using System.Runtime.CompilerServices;

namespace StaticDataLibrary.Attributes;

public sealed class OrderAttribute : Attribute
{
    public int Order { get; }

    public OrderAttribute([CallerLineNumber] int order = 0)
    {
        Order = order;
    }
}