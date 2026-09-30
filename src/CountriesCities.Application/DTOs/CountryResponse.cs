namespace CountriesCities.Application.DTOs;

public class CountryResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
}
