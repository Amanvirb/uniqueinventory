namespace Application.Dto;
public class OrderRouteDto
{
    public string OrderNumber { get; set; }
    public string LocationName { get; set; }
    //public string PartNumber { get; set; }
    //public int AvailableQty { get; set; }
    //public int ReqQty { get; set; }

    public ICollection<RequestedProductDto> ReqProducts { get; set; } = new List<RequestedProductDto>();
}
