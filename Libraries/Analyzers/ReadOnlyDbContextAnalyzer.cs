using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[SuppressMessage("MicrosoftCodeAnalysisReleaseTracking", "RS2001")]
public class ReadOnlyDbContextAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Rule = new(
        DiagnosticIds.ReadOnlyDbContext,
        "Must Use ReadOnlyDbContext",
        "{0}.{1} Rule과 Query에서는 반드시 ReadOnlyDbContext를 사용해야 합니다. ReadOnlyDbContext는 DbContext를 상속받지 않습니다.",
        "Usage",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.FieldDeclaration);
    }

    private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
    {
        var fieldDeclaration = (FieldDeclarationSyntax)context.Node;
        if (fieldDeclaration.Parent is null)
        {
            return;
        }

        var classSymbol = context.SemanticModel.GetDeclaredSymbol(fieldDeclaration.Parent) as INamedTypeSymbol;
        if (classSymbol is null)
        {
            return;
        }

        if (InheritsFrom(classSymbol, new[]{ "IQueryHandler", "IRuleChecker" }))
        {
            var fieldType = fieldDeclaration.Declaration.Type;

            var semanticModel = context.SemanticModel;
            var typeSymbol = semanticModel.GetSymbolInfo(fieldType).Symbol as INamedTypeSymbol;
            if (typeSymbol is null)
            {
                return;
            }

            if (InheritsFrom(typeSymbol, "DbContext"))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, fieldDeclaration.GetLocation(), classSymbol.Name, fieldDeclaration.Declaration.Variables.First().Identifier.ValueText, fieldType));
            }
        }
    }

    // Todo Extension Method
    private static bool InheritsFrom(INamedTypeSymbol typeSymbol, string baseTypeName)
    {
        // 자신이 baseTypeName 라면 true
        if (typeSymbol.Name == baseTypeName)
        {
            return true;
        }

        // 배이스가 baseTypeName 라면 true
        foreach (var baseType in typeSymbol.AllInterfaces.Concat(GetAllBaseTypes(typeSymbol)))
        {
            if (baseType.Name == baseTypeName)
            {
                return true;
            }
        }

        // 재귀
        if (typeSymbol.ContainingType != null)
        {
            return InheritsFrom(typeSymbol.ContainingType, baseTypeName);
        }

        return false;
    }

    private static bool InheritsFrom(INamedTypeSymbol typeSymbol, string[] baseTypeNames)
    {
        // 자신이 baseTypeName 라면 true
        if (baseTypeNames.Contains(typeSymbol.Name))
        {
            return true;
        }

        // 배이스가 baseTypeName 라면 true
        foreach (var baseType in typeSymbol.AllInterfaces.Concat(GetAllBaseTypes(typeSymbol)))
        {
            if (baseTypeNames.Contains(baseType.Name))
            {
                return true;
            }
        }

        // 재귀
        if (typeSymbol.ContainingType != null)
        {
            return InheritsFrom(typeSymbol.ContainingType, baseTypeNames);
        }

        return false;
    }

    // Todo Extension Method
    private static IEnumerable<INamedTypeSymbol> GetAllBaseTypes(INamedTypeSymbol typeSymbol)
    {
        var currentType = typeSymbol.BaseType;

        while (currentType != null)
        {
            yield return currentType;
            currentType = currentType.BaseType;
        }
    }
}
