using BaseApiStuff.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BaseApiStuff.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuth<TUser>(this IServiceCollection serviceCollection) where TUser : IdentityUser
    {
        serviceCollection.AddScoped<IAuthService, BaseAuthService<TUser>>();
        
        return serviceCollection;
    }
}