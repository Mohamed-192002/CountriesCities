namespace CountriesCities.Application.DTOs;

public class CreateCityRequest
{
    public required string Name { get; set; }
    public int CountryId { get; set; }
}
