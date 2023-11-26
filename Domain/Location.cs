namespace Domain;
public class Location
{
    public int Id { get; set; }
    public string LocationName { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
