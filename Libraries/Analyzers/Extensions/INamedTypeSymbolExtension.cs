using Microsoft.CodeAnalysis;

namespace Analyzers.Extensions;

public static class INamedTypeSymbolExtension
{
    public static bool IsInheritedFrom(this INamedTypeSymbol typeSymbol, string baseTypeName)
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
            return typeSymbol.ContainingType.IsInheritedFrom(baseTypeName);
        }

        return false;
    }

    public static bool IsInheritedFrom(this INamedTypeSymbol typeSymbol, string[] baseTypeNames)
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
            return typeSymbol.ContainingType.IsInheritedFrom(baseTypeNames);
        }

        return false;
    }

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
