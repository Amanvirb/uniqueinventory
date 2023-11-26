namespace Domain;
public class PartNumber
{
    public int Id { get; set; }
    public string PartNumberName { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
