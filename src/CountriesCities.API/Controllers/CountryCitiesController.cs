using Microsoft.AspNetCore.Mvc;
using CountriesCities.Application.Interfaces;
using CountriesCities.Application.DTOs;
using CountriesCities.API.Filters;

namespace CountriesCities.API.Controllers;

[ApiController]
[Route("api/countries/{countryId}/cities")]
public class CountryCitiesController : ControllerBase
{
    private readonly ICityService _cityService;
    private readonly ICountryService _countryService;

    public CountryCitiesController(ICityService cityService, ICountryService countryService)
    {
        _cityService = cityService;
        _countryService = countryService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCitiesByCountry(int countryId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var country = await _countryService.GetByIdAsync(countryId, cancellationToken);
        if (country == null)
        {
            return NotFound(new ProblemDetails
            {
                Type = "https://httpstatuses.com/404",
                Title = "Resource Not Found",
                Status = 404,
                Detail = $"Country with id {countryId} was not found."
            });
        }

        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var response = await _cityService.GetByCountryIdPagedAsync(countryId, pageNumber, pageSize, cancellationToken);
        return Ok(response);
    }
}
