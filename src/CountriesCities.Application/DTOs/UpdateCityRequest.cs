namespace CountriesCities.Application.DTOs;

public class UpdateCityRequest
{
    public required string Name { get; set; }
    public int CountryId { get; set; }
}
