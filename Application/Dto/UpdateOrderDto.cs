namespace Application.Dto;
public class UpdateOrderDto
{
    public int Id { get; set; }
    public int OrderNumber { get; set; }

    public bool Confirmed { get; set; }

    public string AppUserId { get; set; }
    public ICollection<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
}
