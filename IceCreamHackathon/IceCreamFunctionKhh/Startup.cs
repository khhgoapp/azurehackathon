using IceCreamFunctionKhh.ExternalDependencies;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(IceCreamFunctionKhh.Startup))]
namespace IceCreamFunctionKhh
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<ProductClient>();
            builder.Services.AddScoped<UserClient>();
        }
    }
}