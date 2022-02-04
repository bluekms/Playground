using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AccountServer.Features
{
    public sealed class HandlerRegistrant
    {
        private readonly IServiceCollection _serviceCollection;
        private readonly List<Type> _types;

        public HandlerRegistrant(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;

            _types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => x.IsPublic)
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .ToList();
        }

        public void Register(Type type)
        {
            var genericTypes = _types
                .Where(x => x.GetInterfaces()
                    .Where(y => y.IsGenericType)
                    .Any(z => z.GetGenericTypeDefinition() == type)).
                ToList();

            foreach (var t in genericTypes)
            {
                var serviceType = t.GetInterfaces().First(i => i.GetGenericTypeDefinition() == type);
                _serviceCollection.AddTransient(serviceType, t);
            }
        }
    }
}