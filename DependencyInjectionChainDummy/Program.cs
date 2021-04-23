using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DependencyInjectionChainDummy.Internal;
using EvilBaschdi.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionChainDummy
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            //IDemoInterface demoInterface = new DemoClass();
            //INumber chainHelperForStringOne = new One();
            //INumber chainHelperForStringTwo = new Two(chainHelperForStringOne);
            //IReturn returnClass = new Return(demoInterface, chainHelperForStringTwo);

            //Console.WriteLine(returnClass.Value);

            var serviceCollection = new ServiceCollection();

            IConfigureChain configureChain = new ConfigureChain();
            configureChain.RunFor(serviceCollection);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IReturn returnClassByDependencyInjection = serviceProvider.GetService<IReturn>();

            if (returnClassByDependencyInjection != null)
            {
                Console.WriteLine(returnClassByDependencyInjection.Value);
            }

            Console.WriteLine("...");
            Console.ReadLine();
        }
    }

    /// <inheritdoc />
    public interface IConfigureChain : IRunFor<IServiceCollection>
    {
    }

    public class ConfigureChain : IConfigureChain
    {
        /// <inheritdoc />
        public void RunFor(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            //services.AddChained<INumber>(typeof(One));
            //services.AddChained<INumber>(typeof(Two));

            services.Chain<INumber>()
                    .Add<Two>()
                    .Add<One>()
                    .Configure();

            services.AddScoped<IDemoInterface, DemoClass>();
            services.AddScoped<IReturn, Return>();
        }
    }

    public static class ChainConfigurator
    {
        public static IChainConfigurator<T> Chain<T>(this IServiceCollection services)
            where T : class
        {
            return new ChainConfiguratorImplementation<T>(services);
        }

        public interface IChainConfigurator<in T>
        {
            IChainConfigurator<T> Add<TImplementation>()
                where TImplementation : T;

            void Configure();
        }

        private class ChainConfiguratorImplementation<T> : IChainConfigurator<T>
            where T : class
        {
            private readonly Type _interfaceType;
            private readonly IServiceCollection _services;
            private readonly List<Type> _types;

            public ChainConfiguratorImplementation(IServiceCollection services)
            {
                _services = services;
                _types = new List<Type>();
                _interfaceType = typeof(T);
            }

            public IChainConfigurator<T> Add<TImplementation>()
                where TImplementation : T
            {
                var type = typeof(TImplementation);

                _types.Add(type);

                return this;
            }

            public void Configure()
            {
                if (_types.Count == 0)
                {
                    throw new InvalidOperationException($"No implementation defined for {_interfaceType.Name}");
                }

                foreach (var type in _types)
                {
                    ConfigureType(type);
                }
            }

            private void ConfigureType(Type currentType)
            {
                // gets the next type, as that will be injected in the current type
                var nextType = _types.SkipWhile(x => x != currentType).SkipWhile(x => x == currentType).FirstOrDefault();

                // Makes a parameter expression, that is the IServiceProvider x 
                var parameter = Expression.Parameter(typeof(IServiceProvider), "x");

                // get constructor with highest number of parameters. Ideally, there should be only 1 constructor, but better be safe.
                var ctor = currentType.GetConstructors().OrderByDescending(x => x.GetParameters().Length).First();

                // for each parameter in the constructor
                var ctorParameters = ctor.GetParameters().Select(p =>
                                                                 {
                                                                     // check if it implements the interface. That's how we find which parameter to inject the next handler.
                                                                     if (!_interfaceType.IsAssignableFrom(p.ParameterType))
                                                                     {
                                                                         // this is a parameter we don't care about, so we just ask GetRequiredService to resolve it for us 
                                                                         return (Expression) Expression.Call(typeof(ServiceProviderServiceExtensions), "GetRequiredService",
                                                                             new[] { p.ParameterType }, parameter);
                                                                     }

                                                                     if (nextType is null)
                                                                     {
                                                                         // if there's no next type, current type is the last in the chain, so it just receives null
                                                                         return Expression.Constant(null, _interfaceType);
                                                                     }

                                                                     // if there is, then we call IServiceProvider.GetRequiredService to resolve next type for us
                                                                     return Expression.Call(typeof(ServiceProviderServiceExtensions), "GetRequiredService", new[] { nextType },
                                                                         parameter);
                                                                 });

                // cool, we have all of our constructors parameters set, so we build a "new" expression to invoke it.
                var body = Expression.New(ctor, ctorParameters.ToArray());

                // if current type is the first in our list, then we register it by the interface, otherwise by the concrete type
                var first = _types[0] == currentType;
                var resolveType = first ? _interfaceType : currentType;
                var expressionType = Expression.GetFuncType(typeof(IServiceProvider), resolveType);

                // finally, we can build our expression
                var expression = Expression.Lambda(expressionType, body, parameter);

                // compile it
                var compiledExpression = (Func<IServiceProvider, object>) expression.Compile();

                // and register it in the services collection as transient
                _services.AddTransient(resolveType, compiledExpression);
            }
        }
    }
}