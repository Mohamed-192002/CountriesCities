using Microsoft.AspNetCore.Mvc;
using CountriesCities.Application.Interfaces;
using CountriesCities.Application.DTOs;
using CountriesCities.API.Filters;

namespace CountriesCities.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(ValidationFilterAttribute))]
public class CitiesController : ControllerBase
{
    private readonly ICityService _cityService;
    private readonly ICountryService _countryService;

    public CitiesController(ICityService cityService, ICountryService countryService)
    {
        _cityService = cityService;
        _countryService = countryService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateCityRequest request, CancellationToken cancellationToken)
    {
        var country = await _countryService.GetByIdAsync(request.CountryId, cancellationToken);
        if (country == null)
        {
            return NotFound(new ProblemDetails
            {
                Type = "https://httpstatuses.com/404",
                Title = "Resource Not Found",
                Status = 404,
                Detail = $"Country with id {request.CountryId} was not found."
            });
        }

        var response = await _cityService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var response = await _cityService.GetByIdAsync(id, cancellationToken);
        
        if (response == null)
        {
            return NotFound(new ProblemDetails
            {
                Type = "https://httpstatuses.com/404",
                Title = "Resource Not Found",
                Status = 404,
                Detail = $"City with id {id} was not found."
            });
        }

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null, CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var response = await _cityService.GetPagedAsync(pageNumber, pageSize, search, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCityRequest request, CancellationToken cancellationToken)
    {
        var country = await _countryService.GetByIdAsync(request.CountryId, cancellationToken);
        if (country == null)
        {
            return NotFound(new ProblemDetails
            {
                Type = "https://httpstatuses.com/404",
                Title = "Resource Not Found",
                Status = 404,
                Detail = $"Country with id {request.CountryId} was not found."
            });
        }

        var success = await _cityService.UpdateAsync(id, request, cancellationToken);

        if (!success)
        {
            return NotFound(new ProblemDetails
            {
                Type = "https://httpstatuses.com/404",
                Title = "Resource Not Found",
                Status = 404,
                Detail = $"City with id {id} was not found."
            });
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var success = await _cityService.DeleteAsync(id, cancellationToken);

        if (!success)
        {
            return NotFound(new ProblemDetails
            {
                Type = "https://httpstatuses.com/404",
                Title = "Resource Not Found",
                Status = 404,
                Detail = $"City with id {id} was not found."
            });
        }

        return NoContent();
    }
}
