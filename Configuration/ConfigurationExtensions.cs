using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Cqc.Helpers.GraphClient.Configuration
{
    public static class ConfigurationExtensions
    {
        public static void SetGraphApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GraphApiConfiguration>(options => configuration.GetSection("GraphApi").Bind(options));
            services.TryAddScoped<IGraphClient, GraphApiClient>();
        }
    }
}
