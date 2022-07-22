using System;
using AuthServer.Models;
using CommonLibrary;
using CommonLibrary.Handlers;
using Shouldly;
using Xunit;
using Xunit.Sdk;

namespace AuthServer.Test.Syntax;

public sealed class HandlerTypeTest
{
    private readonly GenericDerivedTypeSelector typeSelector;
    
    public HandlerTypeTest()
    {
        typeSelector = new GenericDerivedTypeSelector(typeof(AuthServerAssemblyEntryPoint).Assembly);
    }

    [Fact]
    public void MustUseSealedClass()
    {
        OnMustUseSealedClass(typeof(IQueryHandler<,>));
        OnMustUseSealedClass(typeof(ICommandHandler<>));
        OnMustUseSealedClass(typeof(ICommandHandler<,>));
        OnMustUseSealedClass(typeof(IRuleChecker<>));
    }

    private void OnMustUseSealedClass(Type t)
    {
        foreach (var (type, _) in typeSelector.GetGenericInheritedTypes(t))
        {
            try
            {
                type.IsSealed.ShouldBeTrue();
            }
            catch (Exception)
            {
                throw new TestClassException($"[{type.Name}] must be sealed class");
            }
        }
    }
}