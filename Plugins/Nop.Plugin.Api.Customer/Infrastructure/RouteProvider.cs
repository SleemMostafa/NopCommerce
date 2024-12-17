using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Api.Customer.Infrastructure;

public class RouteProvider:IRouteProvider
{
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapControllerRoute(name: "Nop.Plugin.Api.Customer.CheckContactExists",
            pattern: "api/customer/crm",
            defaults: new { controller = "CustomerApi", action = "CheckContactExists" });
    }

    public int Priority => 0;
}