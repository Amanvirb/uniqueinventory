namespace Application.Dto;
public class ProductSearchProductNameDto
{
    public string Location { get; set; }
    public int Quantity { get; set; }
    public List<ConsolidateSerialDto> SerialNumbers { get; set; } = new List<ConsolidateSerialDto>();

}
