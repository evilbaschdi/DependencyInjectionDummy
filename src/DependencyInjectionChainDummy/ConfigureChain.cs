using DependencyInjectionChainDummy.Internal;
using EvilBaschdi.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionChainDummy;

public class ConfigureChain : IConfigureChain
{
    /// <inheritdoc />
    public void RunFor(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

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