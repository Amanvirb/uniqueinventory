namespace Application.Dto;
public class UpdateOrderDto
{
    public int Id { get; set; }
    public bool Confirmed { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }

}
