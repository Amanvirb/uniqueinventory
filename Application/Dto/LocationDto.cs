namespace Application.Dto;
public class LocationDto
{
    public int Id { get; set; }
    public string LocationName { get; set; }
    public List<ProductDto> Products { get; set; } = [];
}
