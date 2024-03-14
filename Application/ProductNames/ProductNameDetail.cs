namespace Application.ProductNames;
public class ProductNameDetail
{
    public class Query : IRequest<Result<ProductNameDto>>
    {
        public int Id { get; set; }
    }
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<ProductNameDto>>
    {
        private readonly DataContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<ProductNameDto>> Handle(Query request, CancellationToken ct)
        {
            var product = await _context.ProductNames
                .Include(x => x.Products)
                .ProjectTo<ProductNameDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

            if (product is null) return Result<ProductNameDto>.Failure("Produt does not exist");

            return Result<ProductNameDto>.Success(product);

        }
    }

}
