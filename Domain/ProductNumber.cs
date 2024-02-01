namespace Domain;
public class ProductNumber
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public string Tags { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
