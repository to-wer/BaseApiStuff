using BaseApiStuff.Configurations;
using BaseApiStuff.Data;
using BaseApiStuff.Entities;
using BaseApiStuff.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaseApiStuff.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Full setup of BaseApiStuff
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddBaseApiStuff(this IServiceCollection services, IConfiguration configuration)
    {
        
        
        services.AddBaseApiStuff<AppUser>(configuration);

        return services;
    }
    
    public static IServiceCollection AddBaseApiStuff<TUser>(this IServiceCollection services, IConfiguration configuration)
        where TUser : IdentityUser
    {
        var jwtSettings = configuration.GetSection("JwtSettings");

        services.Configure<SmtpConfiguration>(configuration.GetSection("SmtpConfiguration"));
        
        services.AddDbContext<BaseDbContext<TUser>>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 4;
            })
            .AddEntityFrameworkStores<BaseDbContext<TUser>>()
            .AddDefaultTokenProviders();
        
        // serviceCollection.AddAuth<TUser>();

        services.AddControllers();

        return services;
    }

    private static IServiceCollection AddAuth<TUser>(this IServiceCollection services) where TUser : IdentityUser
    {
        // TODO: DbContext must be configured here
        services.AddScoped<IAuthService, BaseAuthService<TUser>>();
        
        return services;
    }
}