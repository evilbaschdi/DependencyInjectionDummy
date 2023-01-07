using DependencyInjectionChainDummy.Internal;
using EvilBaschdi.Core;
using EvilBaschdi.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionChainDummy
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main()
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

            // ReSharper disable once SuggestVarOrType_SimpleTypes
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
}