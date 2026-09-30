using Microsoft.EntityFrameworkCore;
using CountriesCities.Application.Interfaces;
using CountriesCities.Application.DTOs;
using CountriesCities.Application.Common;
using CountriesCities.Domain;

namespace CountriesCities.Application.Services;

public class CountryService : ICountryService
{
    private readonly IAppDbContext _context;

    public CountryService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<CountryResponse> CreateAsync(CreateCountryRequest request, CancellationToken cancellationToken = default)
    {
        var country = new Country
        {
            Name = request.Name,
            Code = request.Code
        };

        _context.Countries.Add(country);
        await _context.SaveChangesAsync(cancellationToken);

        return new CountryResponse
        {
            Id = country.Id,
            Name = country.Name,
            Code = country.Code
        };
    }

    public async Task<CountryResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Countries
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CountryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResult<CountryResponse>> GetPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default)
    {
        var query = _context.Countries.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => c.Name.Contains(search) || c.Code.Contains(search));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CountryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<CountryResponse>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateCountryRequest request, CancellationToken cancellationToken = default)
    {
        var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        
        if (country == null)
            return false;

        country.Name = request.Name;
        country.Code = request.Code;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        
        if (country == null)
            return false;

        _context.Countries.Remove(country);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
