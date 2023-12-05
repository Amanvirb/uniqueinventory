namespace Domain;
public class OrderDetail
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    public int PartNumberId { get; set; }
    public PartNumber PartNumber { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
}
