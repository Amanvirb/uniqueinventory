namespace Application.Locations;
public class LocationList
{
    public class Query : IRequest<Result<List<LocationDto>>>
    {
        public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<List<LocationDto>>>
        {
            private readonly DataContext _context = context;
            private readonly IMapper _mapper = mapper;

            public async Task<Result<List<LocationDto>>> Handle(Query request, CancellationToken ct)
            {
                var locations = await _context.Locations
                    .Include(x => x.Products)
                    .ProjectTo<LocationDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);

                return Result<List<LocationDto>>.Success(locations);

            }
        }
    }
}
