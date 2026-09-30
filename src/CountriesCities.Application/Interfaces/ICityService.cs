using CountriesCities.Application.DTOs;
using CountriesCities.Application.Common;

namespace CountriesCities.Application.Interfaces;

public interface ICityService
{
    Task<CityResponse> CreateAsync(CreateCityRequest request, CancellationToken cancellationToken = default);
    Task<CityResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<CityResponse>> GetPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, UpdateCityRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<CityResponse>> GetByCountryIdPagedAsync(int countryId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
