using System;
using System.Windows;
using JetBrains.Annotations;
using MahApps.Metro.Controls.Dialogs;
using MahAppsMetroDependencyInjectionDummy.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MahAppsMetroDependencyInjectionDummy
{
    // ReSharper disable once RedundantExtendsListEntry
    public partial class App : Application
    {
        private readonly IHost _host;

        /// <inheritdoc />
        public App()
        {
            _host = Host.CreateDefaultBuilder()
                        .ConfigureServices((_, services) => { ConfigureServices(services); })
                        .Build();

            ServiceProvider = _host.Services;
        }

        /// <summary>
        ///     ServiceProvider for DependencyInjection
        /// </summary>
        public static IServiceProvider ServiceProvider { get; private set; }


        /// <inheritdoc />
        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();


            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();

            mainWindow.Show();
        }

        private void ConfigureServices([NotNull] IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton(_ => DialogCoordinator.Instance);
            services.AddScoped<IDummyInterface, DummyClass>();
            services.AddScoped<ISomeOtherInterface, SomeOtherClass>();

            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient(typeof(MainWindow));
        }

        /// <inheritdoc />
        protected override async void OnExit([NotNull] ExitEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }
    }
}