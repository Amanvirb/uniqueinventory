namespace Domain;

public class Product
{
    public int Id { get; set; }
    public string SerialNumber { get; set; }

    public int ProductNumberId { get; set; }
    public ProductNumber ProductNumber { get; set; }
    public int LocationId { get; set; }
    public Location Location { get; set; }
}
