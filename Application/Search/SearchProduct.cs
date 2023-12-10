using Application.Dto;

namespace Application.Search;
public class SearchProduct
{
    public class Query : IRequest<Result<List<ProductSearchDto>>>
    {
        public ProductSearchParams Params { get; set; }
        public string Name { get; set; }
    }
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<List<ProductSearchDto>>>
    {
        private readonly DataContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<List<ProductSearchDto>>> Handle(Query request, CancellationToken cancellationToken)
        {

            var output = new List<ProductSearchDto>();

            List<Product> searchedProducts = new();

            var products = await _context.Products
                  .Include(x => x.Location)
                  .Include(x => x.ProductNumber)
                  .Where(x => x.ProductNumber.Name == request.Name)
                  .ToListAsync();

            searchedProducts.AddRange(products);


            var locations = searchedProducts.Select(x => x.Location.Name).Distinct().ToList();

            foreach (var location in locations)
            {
                //var products = searchedProducts
                //     .Where(x => x.Location.Name == location)
                //     .ToList();

                var serials = products
                   .Where(x => x.Location.Name == location)
                   .Select((p) => p.SerialNumber)
                   .ToList();

                var partNumberNames = products.Select(x => x.ProductNumber.Name).Distinct().ToList();

                var searchProd = new List<ProductSearchProductNameDto>();

                foreach (var partNumberName in partNumberNames)
                {
                    searchProd.Add(new()
                    {
                        Location = location,
                        SerialNos = serials
                    });

                    output.Add(new()
                    {
                        ProductName = partNumberName,
                        Quantity = products.Where(x => x.ProductNumber.Name == partNumberName).Count(),
                    });
                }


            }
            return Result<List<ProductSearchDto>>.Success(output);
        }
    }
}