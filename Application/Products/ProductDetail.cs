using FluentValidation;

namespace Application.Products;

public class ProductDetail
{
    public class Query : IRequest<Result<ProductNameDetailDto>>
    {
        public int Id { get; set; }
    }
    public class CommandValidator : AbstractValidator<Query>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<ProductNameDetailDto>>
    {
        private readonly DataContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<ProductNameDetailDto>> Handle(Query request, CancellationToken ct)
        {
            var product = await _context.ProductNames
                .Include(x => x.Products)
                .ProjectTo<ProductNameDetailDto>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

            if (product is null) return Result<ProductNameDetailDto>.Failure("Product not found");

            return Result<ProductNameDetailDto>.Success(product);
        }
    }
}
