namespace Domain;

public class Product
{
    public int Id { get; set; }
    public string SerialNumber { get; set; }

    public int PartNumberId { get; set; }
    public PartNumber PartNumber { get; set; }
    public int LocationId { get; set; }
    public Location Location { get; set; }
}
