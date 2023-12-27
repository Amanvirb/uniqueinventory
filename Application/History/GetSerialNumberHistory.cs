


namespace Application.History;
public class GetSerialNumberHistory
{
    public class Query : IRequest<Result<List<SerialNumberHistoryDto>>>
    {
        public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<List<SerialNumberHistoryDto>>>
        {
            private readonly DataContext _context = context;
            private readonly IMapper _mapper = mapper;

            public async Task<Result<List<SerialNumberHistoryDto>>> Handle(Query request, CancellationToken ct)
            {
                var serialNumberHistory = await _context.SerialNumberHistories
               .ProjectTo<SerialNumberHistoryDto>(_mapper.ConfigurationProvider)
               .Take(100)
               .ToListAsync(ct);

                return Result<List<SerialNumberHistoryDto>>.Success(serialNumberHistory);
            }
        }

    }
}
