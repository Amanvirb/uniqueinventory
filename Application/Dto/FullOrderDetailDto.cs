namespace Application.Dto;
public class FullOrderDetailDto
{
    public string OrderId { get; set; }
    public bool Confirmed { get; set; }
    public bool Packed { get; set; }
    public string UserName { get; set; }
    public ICollection<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
}
