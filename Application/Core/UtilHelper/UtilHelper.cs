namespace Application.Core.UtilHelper;
public static class UtilHelper
{
    public static List<ConsolidationPickDto> GenConsolidatePick(string productName, List<Product> products)
    {
        var locations = products.Select(x => x.Location).Distinct().ToList();

        var output = new List<ConsolidationPickDto>();

        foreach (var location in locations)
        {
            var serials = products
                .Where(x => x.Location == location)
                .Select((p) => new ConsolidateSerialDto
                {
                    SerialNo = p.SerialNumber
                })
                .ToList();

            output.Add(new()
            {
                LocationName = location.Name,
                Serials = serials,
                ProductName = productName,
            });

        }

        return output;
    }
}
