namespace Application.Dto;
public class ConsolidationPickDto
{
    public string LocationName { get; set; }
    public string ProductName { get; set; }
    public ICollection<ConsolidateSerialDto> Serials { get; set; }
}
