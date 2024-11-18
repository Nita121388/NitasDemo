using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WPFArchitectureDemo.Data.DatabaseContexts;
using WPFArchitectureDemo.Data.RepositoryFactory;

namespace WPFArchitectureDemo.UI.Common.HostBuilders
{
    public static class AddDbContextHostBuilderExtensions
    {
        public static IHostBuilder AddDbContext(this IHostBuilder host)
        {
            host.ConfigureServices((context, services) =>
            {
                string connectionString = context.Configuration.GetConnectionString("sqlite");
                Action<DbContextOptionsBuilder> configureDbContext = o => o.UseSqlite(connectionString);

                // 注册 DbContext
                services.AddDbContext<PromptDbContext>(configureDbContext);
                services.AddSingleton<DbContextFactory>(sp => new DbContextFactory(configureDbContext));
                services.AddSingleton<RepositoryFactory>(sp => new RepositoryFactory(configureDbContext));
            });
            return host;
        }
    }
}
