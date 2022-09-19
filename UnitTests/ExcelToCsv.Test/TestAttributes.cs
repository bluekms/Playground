namespace ExcelToCsv.Test;

internal sealed class FactEnvironmentVariableAttribute : FactAttribute
{
    public FactEnvironmentVariableAttribute(string environmentVariable)
    {
        var evn = Environment.GetEnvironmentVariable(environmentVariable);
        if (evn == null)
        {
            Skip = $"Skip. Is Not {environmentVariable}";
        }
    }
}

internal sealed class FactWithoutEnvironmentVariableAttribute : FactAttribute
{
    public FactWithoutEnvironmentVariableAttribute(string environmentVariable)
    {
        var evn = Environment.GetEnvironmentVariable(environmentVariable);
        if (evn != null)
        {
            Skip = $"Skip. Without {environmentVariable}";
        }
    }
}

internal sealed class TheoryEnvironmentVariableAttribute : TheoryAttribute
{
    public TheoryEnvironmentVariableAttribute(string environmentVariable)
    {
        var evn = Environment.GetEnvironmentVariable(environmentVariable);
        if (evn == null)
        {
            Skip = $"Skip. Is Not {environmentVariable}";
        }
    }
}

internal sealed class TheoryWithoutEnvironmentVariableAttribute : TheoryAttribute
{
    public TheoryWithoutEnvironmentVariableAttribute(string environmentVariable)
    {
        var evn = Environment.GetEnvironmentVariable(environmentVariable);
        if (evn != null)
        {
            Skip = $"Skip. Without {environmentVariable}";
        }
    }
}