using System.Reflection;

namespace CommonLibrary;

public sealed record TypeDerivation(Type Type, Type Deriving);

public sealed class GenericDerivedTypeSelector
{
    private readonly Assembly[] assemblies;

    public GenericDerivedTypeSelector(params Assembly[] assemblies)
    {
        this.assemblies = assemblies;
    }

    public IEnumerable<Type> GetInheritedTypes(Type baseType)
    {
        if (baseType.IsGenericType && !baseType.IsConstructedGenericType)
        {
            throw new ArgumentException("must not open generic types", nameof(baseType));
        }

        foreach (var type in GetActivatableTypes().Where(x => x != baseType))
        {
            if (type.IsAssignableTo(baseType))
            {
                yield return type;
            }
        }
    }

    public IEnumerable<TypeDerivation> GetGenericInheritedTypes(Type baseType)
    {
        if (!baseType.IsGenericType || baseType.IsConstructedGenericType)
        {
            throw new ArgumentException("must open generic types", nameof(baseType));
        }

        foreach (var type in GetActivatableTypes().Where(x => x != baseType))
        {
            var types = GetDerivingTypes(type)
                .Where(x => x.IsGenericType)
                .Where(x => x.IsConstructedGenericType);

            foreach (var deriving in types)
            {
                var definition = deriving.GetGenericTypeDefinition();
                if (definition == baseType)
                {
                    yield return new(type, deriving);
                }
            }
        }
    }

    private static IEnumerable<Type> GetDerivingTypes(Type type)
    {
        foreach (var i in type.GetInterfaces())
        {
            yield return i;
        }

        for (var current = type.BaseType; current != null; current = current.BaseType)
        {
            yield return current;
        }
    }

    private IEnumerable<Type> GetActivatableTypes()
    {
        return assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(x => x.IsClass)
            .Where(x => !x.IsAbstract)
            .Where(x => !x.IsGenericType);
    }
}
