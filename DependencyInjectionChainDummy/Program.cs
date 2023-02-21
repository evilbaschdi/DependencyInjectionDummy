using DependencyInjectionChainDummy.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionChainDummy;

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