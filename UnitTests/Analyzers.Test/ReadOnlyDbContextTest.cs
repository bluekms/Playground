using Microsoft.CodeAnalysis.CSharp.Testing.XUnit;
using Microsoft.CodeAnalysis.Testing;

namespace Analyzers.Test;

public class ReadOnlyDbContextTest
{
    [Fact]
    public async Task InRuleCheckerSuccess()
    {
        // TODO remove IsExternalInit (다음 링크 참조)
        // https://stackoverflow.com/questions/62648189/testing-c-sharp-9-0-in-vs2019-cs0518-isexternalinit-is-not-defined-or-imported
        var code = @"
using System.ComponentModel;
namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class IsExternalInit{}
}

public class ReadOnlyAuthDbContext {}

public interface IRule {}
public sealed record FooRule() : IRule;

public interface IRuleChecker<in TRule> where TRule : IRule {}
public sealed class FooRuleChecker : IRuleChecker<FooRule>
{
    private readonly ReadOnlyAuthDbContext dbContext;
}
";
        var expected = DiagnosticResult.EmptyDiagnosticResults;
        await AnalyzerVerifier<ReadOnlyDbContextAnalyzer>.VerifyAnalyzerAsync(code, expected);
    }

    [Fact]
    public async Task InRuleCheckerFailure()
    {
        // TODO remove IsExternalInit (다음 링크 참조)
        // https://stackoverflow.com/questions/62648189/testing-c-sharp-9-0-in-vs2019-cs0518-isexternalinit-is-not-defined-or-imported
        var code = @"
using System.ComponentModel;
namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class IsExternalInit{}
}

public class DbContext {}
public class AuthDbContext : DbContext {}

public interface IRule {}
public sealed record FooRule() : IRule;

public interface IRuleChecker<in TRule> where TRule : IRule {}
public sealed class FooRuleChecker : IRuleChecker<FooRule>
{
    private readonly AuthDbContext dbContext;
}
";

        var rule = ReadOnlyDbContextAnalyzer.Rule;
        var expected = new DiagnosticResult(rule.Id, rule.DefaultSeverity)
            .WithLocation("/0/Test0.cs", 18, 5)
            .WithMessageFormat(rule.MessageFormat)
            .WithArguments("FooRuleChecker", "dbContext", "AuthDbContext");

        await AnalyzerVerifier<ReadOnlyDbContextAnalyzer>.VerifyAnalyzerAsync(code, expected);
    }

    [Fact]
    public async Task InQueryHandlerSuccess()
    {
        // TODO remove IsExternalInit (다음 링크 참조)
        // https://stackoverflow.com/questions/62648189/testing-c-sharp-9-0-in-vs2019-cs0518-isexternalinit-is-not-defined-or-imported
        var code = @"
using System.ComponentModel;
namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class IsExternalInit{}
}

public class ReadOnlyAuthDbContext {}

public interface IQuery {}
public sealed record FooQuery() : IQuery;

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery {}
public sealed class FooQueryHandler : IQueryHandler<FooQuery, int>
{
    private readonly ReadOnlyAuthDbContext dbContext;
}
";
        var expected = DiagnosticResult.EmptyDiagnosticResults;
        await AnalyzerVerifier<ReadOnlyDbContextAnalyzer>.VerifyAnalyzerAsync(code, expected);
    }

    [Fact]
    public async Task InQueryHandlerFailure()
    {
        // TODO remove IsExternalInit (다음 링크 참조)
        // https://stackoverflow.com/questions/62648189/testing-c-sharp-9-0-in-vs2019-cs0518-isexternalinit-is-not-defined-or-imported
        var code = @"
using System.ComponentModel;
namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class IsExternalInit{}
}

public class DbContext {}
public class AuthDbContext : DbContext {}

public interface IQuery {}
public sealed record FooQuery() : IQuery;

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery {}
public sealed class FooQueryHandler : IQueryHandler<FooQuery, int>
{
    private readonly AuthDbContext dbContext;
}
";
        var rule = ReadOnlyDbContextAnalyzer.Rule;
        var expected = new DiagnosticResult(rule.Id, rule.DefaultSeverity)
            .WithLocation("/0/Test0.cs", 18, 5)
            .WithMessageFormat(rule.MessageFormat)
            .WithArguments("FooQueryHandler", "dbContext", "AuthDbContext");

        await AnalyzerVerifier<ReadOnlyDbContextAnalyzer>.VerifyAnalyzerAsync(code, expected);
    }
}
