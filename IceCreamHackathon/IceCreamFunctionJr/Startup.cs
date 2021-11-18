using IceCreamFunctionJr;
using IceCreamFunctionJr.ExternalDependencies.Products;
using IceCreamFunctionJr.ExternalDependencies.UserRatings;
using IceCreamFunctionJr.ExternalDependencies.Users;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace IceCreamFunctionJr
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<ProductClient>();
            builder.Services.AddScoped<UserClient>();
            builder.Services.AddScoped<UserRatingsClient>();
        }
    }
}