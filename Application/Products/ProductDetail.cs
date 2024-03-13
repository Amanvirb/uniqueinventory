using FluentValidation;

namespace Application.Products;

public class ProductDetail
{
    public class Query : IRequest<Result<ProductDetailDto>>
    {
        public string SerialNo { get; set; }
    }
    public class CommandValidator : AbstractValidator<Query>
    {
        public CommandValidator()
        {
            RuleFor(x => x.SerialNo).NotEmpty();
        }
    }
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<ProductDetailDto>>
    {
        private readonly DataContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<ProductDetailDto>> Handle(Query request, CancellationToken ct)
        {
            var product = await _context.Products
                .ProjectTo<ProductDetailDto>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(x => x.SerialNumber == request.SerialNo, ct);

            if (product is null) return Result<ProductDetailDto>.Failure("Product not found");

            return Result<ProductDetailDto>.Success(product);
        }
    }
}
