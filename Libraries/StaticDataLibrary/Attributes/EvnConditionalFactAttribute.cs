using Xunit;

namespace StaticDataLibrary.Attributes;

public sealed class EvnConditionalFactAttribute<T> : FactAttribute
{
    public EvnConditionalFactAttribute(string evn, T runCondition)
    {
        var value = Environment.GetEnvironmentVariable(evn);
        if (string.IsNullOrWhiteSpace(value) || Equals(runCondition, value))
        {
            return;
        }
        
        Skip = $"Skip. {evn}({value}) is not {runCondition}";
    }
}
