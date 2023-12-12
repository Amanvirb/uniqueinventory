namespace Application.Dto;
public class ProductSearchDto
{
    public string ProductName { get; set; }
    public ICollection<ProductSearchProductNameDto> Products { get; set; } = new List<ProductSearchProductNameDto>();

}
