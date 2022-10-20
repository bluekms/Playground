using Xunit;

namespace StaticDataLibrary.Attributes;

public sealed class EvnConditionalFactAttribute : FactAttribute
{
    public EvnConditionalFactAttribute(string environmentVariable)
    {
        var evn = Environment.GetEnvironmentVariable(environmentVariable);
        if (evn != null)
        {
            Skip = $"Skip. Without {environmentVariable}";
        }
    }
}
