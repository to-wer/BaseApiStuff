using BaseApiStuff.Entities;
using BaseApiStuff.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BaseApiStuff.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBaseApiStuff(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddBaseApiStuff<AppUser>();

        return serviceCollection;
    }
    
    public static IServiceCollection AddBaseApiStuff<TUser>(this IServiceCollection serviceCollection)
        where TUser : IdentityUser
    {
        // serviceCollection.AddAuth<TUser>();

        serviceCollection.AddControllers();

        return serviceCollection;
    }

    private static IServiceCollection AddAuth<TUser>(this IServiceCollection serviceCollection) where TUser : IdentityUser
    {
        // TODO: DbContext must be configured here
        serviceCollection.AddScoped<IAuthService, BaseAuthService<TUser>>();
        
        return serviceCollection;
    }
}