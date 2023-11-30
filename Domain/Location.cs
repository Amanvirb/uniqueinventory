namespace Domain;
public class Location
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int TotalCapacity { get; set; } = 100;
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
