namespace Domain;

public class Product
{
    public int Id { get; set; }
    public string SerialNumber { get; set; }
    public string PartNumberName { get; set; }
    public int LocationId { get; set; }
    public Location Location { get; set; }
}
