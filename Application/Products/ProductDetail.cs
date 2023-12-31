﻿using FluentValidation;

namespace Application.Products;

public class ProductDetail
{
    public class Query : IRequest<Result<ProductDto>>
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
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<ProductDto>>
    {
        private readonly DataContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<ProductDto>> Handle(Query request, CancellationToken ct)
        {
            var product = await _context.Products
                .Include(x => x.Location)
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

            if (product is null) return Result<ProductDto>.Failure("Product not found");

            return Result<ProductDto>.Success(product);
        }
    }
}
