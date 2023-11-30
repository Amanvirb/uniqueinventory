namespace Application.Dto;
public class ConsolidationDto
{
    public string LocationName { get; set; }
    public int TotalCapacity { get; set; }
    public int TotalProducts { get; set; }
    public int EneteredPartNumberTotalCount { get; set; }
    public int EmptySpace { get; set; }
    public string PartNumberName { get; set; }
    public ICollection<ConsolidateSerialDto> Serials { get; set; }
}
