namespace Application.Locations;
public class ProductNameDetail
{
    public class Query : IRequest<Result<ProductNumberDto>>
    {
        public string Name { get; set; }
    }
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<ProductNumberDto>>
    {
        private readonly DataContext _context = context;
        private readonly IMapper _mapper = mapper;


        public async Task<Result<ProductNumberDto>> Handle(Query request, CancellationToken ct)
        {
            var product = await _context.ProductNumbers
                .Include(x => x.Products)
                .ProjectTo<ProductNumberDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Name == request.Name.Trim().ToUpper(), ct);

            if (product is null) return null;

            return Result<ProductNumberDto>.Success(product);

        }
    }

}
