namespace Application.Orders;
public class OrderList
{
    public class Query : IRequest<Result<List<FullOrderDetailDto>>>
    {
        public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<List<FullOrderDetailDto>>>
        {
            private readonly DataContext _context = context;
            private readonly IMapper _mapper = mapper;

            public async Task<Result<List<FullOrderDetailDto>>> Handle(Query request, CancellationToken ct)
            {
                var dbOrders = await _context.Orders
                   .Include(a => a.AppUser)
                   .Include(o => o.OrderDetails)
                    .ProjectTo<FullOrderDetailDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(ct);

               return Result<List<FullOrderDetailDto>>.Success(dbOrders);

            }
        }
    }
}