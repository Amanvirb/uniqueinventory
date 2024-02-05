namespace Domain;

public class Product
{
    public int Id { get; set; }
    public string SerialNumber { get; set; }

    public int ProductNameId { get; set; }
    public ProductName ProductName { get; set; }
    public int LocationId { get; set; }
    public Location Location { get; set; }
}
