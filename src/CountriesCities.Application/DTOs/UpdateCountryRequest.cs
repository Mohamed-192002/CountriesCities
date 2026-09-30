namespace CountriesCities.Application.DTOs;

public class UpdateCountryRequest
{
    public required string Name { get; set; }
    public required string Code { get; set; }
}
