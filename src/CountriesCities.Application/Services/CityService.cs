using Microsoft.EntityFrameworkCore;
using CountriesCities.Application.Interfaces;
using CountriesCities.Application.DTOs;
using CountriesCities.Application.Common;
using CountriesCities.Domain;

namespace CountriesCities.Application.Services;

public class CityService : ICityService
{
    private readonly IAppDbContext _context;

    public CityService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<CityResponse> CreateAsync(CreateCityRequest request, CancellationToken cancellationToken = default)
    {
        var city = new City
        {
            Name = request.Name,
            CountryId = request.CountryId
        };

        _context.Cities.Add(city);
        await _context.SaveChangesAsync(cancellationToken);

        // Load the country name for the response
        var countryName = await _context.Countries
            .Where(c => c.Id == city.CountryId)
            .Select(c => c.Name)
            .FirstOrDefaultAsync(cancellationToken);

        return new CityResponse
        {
            Id = city.Id,
            Name = city.Name,
            CountryId = city.CountryId,
            CountryName = countryName ?? string.Empty
        };
    }

    public async Task<CityResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Cities
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CityResponse
            {
                Id = c.Id,
                Name = c.Name,
                CountryId = c.CountryId,
                CountryName = c.Country!.Name
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResult<CityResponse>> GetPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default)
    {
        var query = _context.Cities.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => c.Name.Contains(search));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CityResponse
            {
                Id = c.Id,
                Name = c.Name,
                CountryId = c.CountryId,
                CountryName = c.Country!.Name
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<CityResponse>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateCityRequest request, CancellationToken cancellationToken = default)
    {
        var city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        
        if (city == null)
            return false;

        city.Name = request.Name;
        city.CountryId = request.CountryId;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        
        if (city == null)
            return false;

        _context.Cities.Remove(city);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<PagedResult<CityResponse>> GetByCountryIdPagedAsync(int countryId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Cities.AsNoTracking().Where(c => c.CountryId == countryId);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CityResponse
            {
                Id = c.Id,
                Name = c.Name,
                CountryId = c.CountryId,
                CountryName = c.Country!.Name
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<CityResponse>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
