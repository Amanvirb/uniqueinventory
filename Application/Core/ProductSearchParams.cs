namespace Application.Core;
public class ProductSearchParams : PagingParams

{
    public string ProductName { get; set; }
    public string Location { get; set; }
    public string SerialNo { get; set; }
}
