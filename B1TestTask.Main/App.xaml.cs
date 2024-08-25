using B1TestTask.Main.Data;
using B1TestTask.Main.Services;
using B1TestTask.Main.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using Application = System.Windows.Application;

namespace B1TestTask.Main
{
    public partial class App : Application
    {
        private static IHost _host;

        public static IHost Host => _host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        public static IServiceProvider Services => Host.Services;

        internal static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            var connectionstring = host.Configuration.GetSection("Database");

            services
            .AddDatabase(host.Configuration.GetSection("Database"))
            .AddRepositoryInDb()
            .AddService()
            .AddViewModels();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Host;
            base.OnStartup(e);
            await host.StartAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using var host = Host;
            base.OnExit(e);
            await Host.StopAsync();
        }
    }
}
