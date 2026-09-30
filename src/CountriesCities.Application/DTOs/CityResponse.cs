namespace CountriesCities.Application.DTOs;

public class CityResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int CountryId { get; set; }
    public required string CountryName { get; set; }
}
