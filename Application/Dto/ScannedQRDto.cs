namespace Application.Dto;
public class ScannedQRDto
{
    public int Id { get; set; }
    public string LocationName { get; set; }
    public ICollection<ScannedProduct> Products { get; set; } = new List<ScannedProduct>();

    public int Quantity { get; set; }
    public bool Status { get; set; } = false;
}
