using CountriesCities.Application.DTOs;
using CountriesCities.Application.Common;

namespace CountriesCities.Application.Interfaces;

public interface ICountryService
{
    Task<CountryResponse> CreateAsync(CreateCountryRequest request, CancellationToken cancellationToken = default);
    Task<CountryResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<CountryResponse>> GetPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpdateCountryRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
