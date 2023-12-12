using Application.Dto;
using Application.Extensions;
using Domain;
using System.Collections.Immutable;

namespace Application.ProductSearch;
public class SearchProduct
{
    public class Query : IRequest<Result<List<ProductSearchDto>>>
    {
        public ProductSearchParams Params { get; set; }
    }
    public class Handler(DataContext context) : IRequestHandler<Query, Result<List<ProductSearchDto>>>
    {
        private readonly DataContext _context = context;

        public async Task<Result<List<ProductSearchDto>>> Handle(Query request, CancellationToken ct)
        {
            var param = request.Params;
            
            param.ProductName = param.ProductName?.Trim().ToUpper() ?? string.Empty;
            param.Location = param.Location?.Trim().ToUpper() ?? string.Empty;
            param.SerialNo = param.SerialNo?.Trim().ToUpper() ?? string.Empty;

            var output = new List<ProductSearchDto>();

            List<ProductSearchProductNameDto> searchedProducts = new();

            var query = _context.Products
                 .Include(x => x.Location)
                 .Include(x => x.ProductNumber)
                 .Search(param)
                 .AsQueryable();

            var products = await query.ToListAsync(ct);

            List<ProductSearchProductNameDto> searchedProductsName = new();

            var locations = products.Select(x => x.Location.Name).Distinct().ToList();

            foreach (var location in locations)
            {
                var serials = products
                    .Where(x => x.Location.Name == location)
                    .Select((p) => new ConsolidateSerialDto
                    {
                        SerialNo = p.SerialNumber
                    })
                    .ToList();

                searchedProductsName.Add(new()
                {
                    Location = location,
                    //Quantity = products.Where(x => x.ProductNumber.Name.Contains(param.ProductName)).Count(),
                    Quantity = products.Count(),
                    SerialNumbers = serials,
                });
            }

            var productNames = products.Select(x => x.ProductNumber.Name).Distinct().ToList();

            foreach (var ProductName in productNames)
            {
                output.Add(new()
                {
                    ProductName = ProductName,
                    Products = searchedProductsName
                });
            }
            return Result<List<ProductSearchDto>>.Success(output);

        }
    }
}