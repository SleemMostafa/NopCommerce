using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Api.Customer.Services;

namespace Nop.Plugin.Api.Customer.Infrastructure;

public class NopStartup:INopStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<CustomerCrmService>();
    }

    public void Configure(IApplicationBuilder application)
    {
        
    }

    public int Order => 3000;
}