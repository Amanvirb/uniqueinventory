namespace Domain;
public class SerialNumberHistory
{
    public int Id { get; set; }
    public string SerialNumber { get; set; }
    public string ProductName { get; set; }
    public string LocationName { get; set; }
    public string Remarks { get; set; }

    public DateTime DateTime { get; set; }
}
