using Microsoft.CodeAnalysis.CSharp.Testing.XUnit;
using Microsoft.CodeAnalysis.Testing;

namespace Analyzers.Test;

public class SealedHandlerTest
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

public interface IRule {}
public sealed record FooRule() : IRule;

public interface IRuleChecker<in TRule> where TRule : IRule {}
public sealed class FooRuleChecker : IRuleChecker<FooRule> {}
";
        var expected = DiagnosticResult.EmptyDiagnosticResults;
        await AnalyzerVerifier<SealedHandlerAnalyzer>.VerifyAnalyzerAsync(code, expected);
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

public interface IRule {}
public sealed record FooRule() : IRule;

public interface IRuleChecker<in TRule> where TRule : IRule {}
public class FooRuleChecker : IRuleChecker<FooRule> {}
";
        var rule = SealedHandlerAnalyzer.Rule;
        var expected = new DiagnosticResult(rule.Id, rule.DefaultSeverity)
            .WithLocation("/0/Test0.cs", 13, 14)
            .WithMessageFormat(rule.MessageFormat)
            .WithArguments("<global namespace>", "FooRuleChecker");

        await AnalyzerVerifier<SealedHandlerAnalyzer>.VerifyAnalyzerAsync(code, expected);
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

public interface IQuery {}
public sealed record FooQuery() : IQuery;

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery {}
public sealed class FooQueryHandler : IQueryHandler<FooQuery, int> {}
";
        var expected = DiagnosticResult.EmptyDiagnosticResults;
        await AnalyzerVerifier<SealedHandlerAnalyzer>.VerifyAnalyzerAsync(code, expected);
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

public interface IQuery {}
public sealed record FooQuery() : IQuery;

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery {}
public class FooQueryHandler : IQueryHandler<FooQuery, int> {}
";
        var rule = SealedHandlerAnalyzer.Rule;
        var expected = new DiagnosticResult(rule.Id, rule.DefaultSeverity)
            .WithLocation("/0/Test0.cs", 13, 14)
            .WithMessageFormat(rule.MessageFormat)
            .WithArguments("<global namespace>", "FooQueryHandler");

        await AnalyzerVerifier<SealedHandlerAnalyzer>.VerifyAnalyzerAsync(code, expected);
    }

    [Fact]
    public async Task InCommandHandlerSuccess()
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

public interface ICommand {}
public sealed record FooCommand() : ICommand;

public interface ICommandHandler<in TCommand> where TCommand : ICommand {}
public sealed class FooCommandHandler : ICommandHandler<FooCommand> {}
";
        var expected = DiagnosticResult.EmptyDiagnosticResults;
        await AnalyzerVerifier<SealedHandlerAnalyzer>.VerifyAnalyzerAsync(code, expected);
    }

    [Fact]
    public async Task InCommandHandlerFailure()
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

public interface ICommand {}
public sealed record FooCommand() : ICommand;

public interface ICommandHandler<in TCommand> where TCommand : ICommand {}
public class FooCommandHandler : ICommandHandler<FooCommand> {}
";
        var rule = SealedHandlerAnalyzer.Rule;
        var expected = new DiagnosticResult(rule.Id, rule.DefaultSeverity)
            .WithLocation("/0/Test0.cs", 13, 14)
            .WithMessageFormat(rule.MessageFormat)
            .WithArguments("<global namespace>", "FooCommandHandler");

        await AnalyzerVerifier<SealedHandlerAnalyzer>.VerifyAnalyzerAsync(code, expected);
    }

    [Fact]
    public async Task InCommandHandler_1Success()
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

public interface ICommand {}
public sealed record FooCommand() : ICommand;

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand {}
public sealed class FooCommandHandler : ICommandHandler<FooCommand, int> {}
";
        var expected = DiagnosticResult.EmptyDiagnosticResults;
        await AnalyzerVerifier<SealedHandlerAnalyzer>.VerifyAnalyzerAsync(code, expected);
    }

    [Fact]
    public async Task InCommandHandler_1Failure()
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

public interface ICommand {}
public sealed record FooCommand() : ICommand;

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand {}
public class FooCommandHandler : ICommandHandler<FooCommand, int> {}
";
        var rule = SealedHandlerAnalyzer.Rule;
        var expected = new DiagnosticResult(rule.Id, rule.DefaultSeverity)
            .WithLocation("/0/Test0.cs", 13, 14)
            .WithMessageFormat(rule.MessageFormat)
            .WithArguments("<global namespace>", "FooCommandHandler");

        await AnalyzerVerifier<SealedHandlerAnalyzer>.VerifyAnalyzerAsync(code, expected);
    }
}
