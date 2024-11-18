using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WPFArchitectureDemo.Business.IManager;
using WPFArchitectureDemo.Business.Manager;
using WPFArchitectureDemo.Data.RepositoryFactory;
using WPFArchitectureDemo.Service.IService;
using WPFArchitectureDemo.Service.Services;

namespace WPFArchitectureDemo.UI.Common.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                // 注册 IPromptManager 和 IPromptService
                services.AddSingleton<IPromptManager, PromptManager>(sp =>
                {
                    var repositoryFactory = sp.GetRequiredService<RepositoryFactory>();
                    return new PromptManager(repositoryFactory);
                });

                services.AddSingleton<IPromptService, PromptService>(sp =>
                {
                    var promptManager = sp.GetRequiredService<IPromptManager>();
                    return new PromptService(promptManager);
                });
            });
            return host;
        }
    }
}
