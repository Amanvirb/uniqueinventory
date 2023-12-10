namespace Application.Products;

public class GetOrderDetail
{
    public class Query : IRequest<Result<FullOrderDetailDto>>
    {
        public int Id { get; set; }
    }
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<FullOrderDetailDto>>
    {
        private DataContext _context = context;
        private IMapper _mapper = mapper;

        public async Task<Result<FullOrderDetailDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var dbOrder = await _context.Orders
                .Include(o => o.AppUser)
                .Include(x => x.OrderDetails)
                .ProjectTo<FullOrderDetailDto>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

            if (dbOrder is null) return Result<FullOrderDetailDto>.Failure("Order not found");

            return Result<FullOrderDetailDto>.Success(dbOrder);
        }
    }
}
