namespace Domain;

public class Product
{
    public int Id { get; set; }
    public string SerialNumber { get; set; }

    public int PartNumberNameId { get; set; }
    public PartNumber PartNumberName { get; set; }
    public int LocationId { get; set; }
    public Location Location { get; set; }
}
