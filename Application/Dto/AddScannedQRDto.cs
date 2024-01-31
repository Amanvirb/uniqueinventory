namespace Application.Dto;
public class AddScannedQRDto
{
    public int Id { get; set; }
    public string LocationName { get; set; }
    public ICollection<AddScannedProduct> Products { get; set; } = new List<AddScannedProduct>();

    public int Quantity { get; set; }
    public bool Status { get; set; } = false;
}
