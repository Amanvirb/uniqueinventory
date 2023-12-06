namespace Application.Orders;
public class OrderList
{
    public class Query : IRequest<Result<List<CreateOrderDto>>>
    {
        public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<List<CreateOrderDto>>>
        {
            private readonly DataContext _context = context;
            private readonly IMapper _mapper = mapper;

            public async Task<Result<List<CreateOrderDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var dbOrders = await _context.Orders
                   //.Include(a => a.AppUser)
                   .Include(o => o.OrderDetails)
                    .ProjectTo<CreateOrderDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken: cancellationToken);

               return Result<List<CreateOrderDto>>.Success(dbOrders);

            }
        }
    }
}