using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[SuppressMessage("MicrosoftCodeAnalysisReleaseTracking", "RS2001", Justification = "내부 분석기를 위한 분석 항목 추가 무시")]
public class SealedHandlerAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Rule = new(
        DiagnosticIds.SealedHandler,
        "Must Use sealed class",
        "{0}.{1} Rule, Query, Command에서는 반드시 sealed class를 사용해야 합니다",
        "Usage",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.ClassDeclaration);
    }

    private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classDeclaration)
        {
            return;
        }

        var semanticModel = context.SemanticModel;
        var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
        if (classSymbol is null)
        {
            return;
        }

        // TODO nameof 로 수정
        if (classSymbol.IsInheritedFrom(new[] { "IRuleChecker", "IQueryHandler", "ICommandHandler" }))
        {
            if (!classSymbol.IsSealed)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    Rule,
                    classDeclaration.Identifier.GetLocation(),
                    classSymbol.ContainingNamespace,
                    classSymbol.Name));
            }
        }
    }
}
