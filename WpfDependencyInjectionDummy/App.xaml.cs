using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WpfDependencyInjectionDummy.Internal;

namespace WpfDependencyInjectionDummy;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
// ReSharper disable once RedundantExtendsListEntry
public partial class App : Application
{
    // ReSharper disable once MemberCanBePrivate.Global
    public IServiceProvider ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        ServiceProvider = serviceCollection.BuildServiceProvider();

        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IDummyInterface, DummyClass>();
        services.AddTransient(typeof(MainWindow));
    }
}