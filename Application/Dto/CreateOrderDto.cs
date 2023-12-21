namespace Application.Dto;
public class CreateOrderDto
{
    public string OrderId { get; set; }
    public bool Confirmed { get; set; }
    public bool Packed { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }

}
