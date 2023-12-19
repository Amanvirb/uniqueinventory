namespace Domain;
public class Order
{
    public int Id { get; set; }
    public string BuyerId { get; set; }
    public int OrderNumber { get; set; }
    public bool Confirmed { get; set; }
    public bool Packed { get; set; }

    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
