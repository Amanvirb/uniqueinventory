namespace Application.Dto;
public class ProductNumberDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public ICollection<ProductDto> Products { get; set; } = new List<ProductDto>();
}
