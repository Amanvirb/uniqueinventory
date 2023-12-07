namespace Application.Dto;
public class ProductNumberDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Quantity { get; set; }

    //public List<LocationDto> Locations { get; set; } = new List<LocationDto>();
    public ICollection<ProductDto> Products { get; set; } = new List<ProductDto>();
}
