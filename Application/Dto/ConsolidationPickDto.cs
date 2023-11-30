namespace Application.Dto;
public class ConsolidationPickDto
{
    public string LocationName { get; set; }
    public string PartNumberName { get; set; }
    public ICollection<ConsolidateSerialDto> Serials { get; set; }
}
