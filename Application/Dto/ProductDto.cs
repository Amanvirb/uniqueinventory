namespace Application.Dto;
public class ProductDto
{
    public int Id { get; set; }
    public string SerialNumber { get; set; }
    public string ProductName { get; set; }
    public string LocationName { get; set; }

    public int Quantity { get; set; }
    public bool Status { get; set; } = false;
}
