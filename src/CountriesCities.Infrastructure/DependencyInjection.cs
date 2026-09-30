using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CountriesCities.Application.Interfaces;
using CountriesCities.Infrastructure.Persistence;
using CountriesCities.Application.Services;

namespace CountriesCities.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<ICityService, CityService>();

        return services;
    }
}
