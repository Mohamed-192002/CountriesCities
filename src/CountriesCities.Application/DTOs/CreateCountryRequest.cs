namespace CountriesCities.Application.DTOs;

public class CreateCountryRequest
{
    public required string Name { get; set; }
    public required string Code { get; set; }
}
