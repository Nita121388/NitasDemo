using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using WPFArchitectureDemo.Data.DatabaseContexts;
using WPFArchitectureDemo.UI.Common;
using WPFArchitectureDemo.UI.Common.HostBuilders;

namespace WPFArchitectureDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;
        public IServiceProvider Services { get; }
        private readonly IHost _host;
        public App()
        {
            _host = CreateHostBuilder().Build();
            Services = _host.Services;
        }

        public static IHostBuilder CreateHostBuilder(string[] args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .AddConfiguration()
                .AddAutoMapper()
                .AddDbContext()
                .AddServices()
                .AddViewModels()
                .AddViews();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _host?.Start();

            //应用所有未应用的迁移到数据库，确保数据库结构是最新的。
            DbContextFactory contextFactory = _host.Services.GetRequiredService<DbContextFactory>();
            using (var context = contextFactory.CreateDbContext())
            {
                context.Database.Migrate();
            }

            Window window = _host.Services.GetRequiredService<MainWindow>();
            window.Show();

        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();

            base.OnExit(e);
        }
    }

}
