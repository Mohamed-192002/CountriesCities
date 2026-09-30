using Microsoft.EntityFrameworkCore;
using CountriesCities.Application.Interfaces;
using CountriesCities.Domain;
using System.Reflection;

namespace CountriesCities.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<City> Cities => Set<City>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        SeedData(modelBuilder);
        
        base.OnModelCreating(modelBuilder);
    }
    
    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>().HasData(
            new Country { Id = 1, Name = "Egypt", Code = "EG" },
            new Country { Id = 2, Name = "Saudi Arabia", Code = "SA" },
            new Country { Id = 3, Name = "United Arab Emirates", Code = "AE" }
        );

        modelBuilder.Entity<City>().HasData(
            new City { Id = 1, Name = "Cairo", CountryId = 1 },
            new City { Id = 2, Name = "Alexandria", CountryId = 1 },
            new City { Id = 3, Name = "Giza", CountryId = 1 },
            new City { Id = 4, Name = "Riyadh", CountryId = 2 },
            new City { Id = 5, Name = "Jeddah", CountryId = 2 },
            new City { Id = 6, Name = "Dubai", CountryId = 3 },
            new City { Id = 7, Name = "Abu Dhabi", CountryId = 3 }
        );
    }
}
