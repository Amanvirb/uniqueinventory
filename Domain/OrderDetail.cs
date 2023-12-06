namespace Domain;
public class OrderDetail
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    public int ProductNumberId { get; set; }
    public ProductNumber ProductNumber { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
}
