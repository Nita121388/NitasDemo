using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WPFArchitectureDemo.UI.Common.HostBuilders
{
    public static class AddAutoMapperHostBuilderExtensions
    {
        public static IHostBuilder AddAutoMapper(this IHostBuilder host)
        {
            host.ConfigureServices((context, services) =>
            {
                services.AddAutoMapper(typeof(App));
            });
            return host;
        }
    }
}
