namespace Application.Dto;
public class OrderRouteDto
{
    public string OrderNumber { get; set; }
    public string LocationName { get; set; }

    public ICollection<RequestedProductDto> ReqProducts { get; set; } = new List<RequestedProductDto>();
}
