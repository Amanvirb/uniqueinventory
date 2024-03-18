namespace Application.Dto;
public class ProductSearchDto
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public ICollection<ProductSearchProductNameDto> Products { get; set; } = new List<ProductSearchProductNameDto>();

}
