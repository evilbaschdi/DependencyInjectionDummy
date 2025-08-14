using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace MahAppsMetroDependencyInjectionDummy;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
// ReSharper disable once RedundantExtendsListEntry
public partial class MainWindow : MetroWindow
{
    private readonly IServiceProvider _serviceProvider;

    public MainWindow()
    {
        InitializeComponent();
        _serviceProvider = App.ServiceProvider;
        Loaded += MainWindowLoaded;
    }

    private void MainWindowLoaded(object sender, RoutedEventArgs e)
    {
        ArgumentNullException.ThrowIfNull(sender);

        ArgumentNullException.ThrowIfNull(e);

        DataContext = ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, typeof(MainWindowViewModel));
    }
}