using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace B1TestTask.Main
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) => 
            {
                config.SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName);
                config.AddJsonFile("appsettings.json", false);
                config.AddJsonFile("appsettings.personal.json", false);
            })
            .ConfigureServices(App.ConfigureServices);
    }
}
