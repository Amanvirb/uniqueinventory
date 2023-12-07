namespace Application.Products;

public class GetOrderDetail
{
    public class Query : IRequest<Result<CreateOrderDto>>
    {
        public int Id { get; set; }
    }
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<CreateOrderDto>>
    {
        private DataContext _context = context;
        private IMapper _mapper = mapper;

        public async Task<Result<CreateOrderDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var dbOrder = await _context.Orders
                .Include(o => o.AppUser)
                .Include(x => x.OrderDetails)
                .ProjectTo<CreateOrderDto>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

            if (dbOrder is null) return Result<CreateOrderDto>.Failure("Order not found");

            return Result<CreateOrderDto>.Success(dbOrder);
        }
    }
}
