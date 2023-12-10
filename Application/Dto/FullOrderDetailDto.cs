namespace Application.Dto;
public class FullOrderDetailDto
{
    public int Id { get; set; }
    public bool Confirmed { get; set; }
    public bool Packed { get; set; }
    public ICollection<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
}
