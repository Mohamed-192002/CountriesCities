using Microsoft.EntityFrameworkCore;
using CountriesCities.Domain;

namespace CountriesCities.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Country> Countries { get; }
    DbSet<City> Cities { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
