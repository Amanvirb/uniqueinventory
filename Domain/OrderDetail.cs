namespace Domain;
public class OrderDetail
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    public int ProductNameId { get; set; }
    public ProductName ProductName { get; set; }
    public string OrderId { get; set; }
    public Order Order { get; set; }
}
