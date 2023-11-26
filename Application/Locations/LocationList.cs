namespace Application.Locations;
public class LocationList
{
    public class Query : IRequest<Result<List<LocationDto>>>
    {
        public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<List<LocationDto>>>
        {
            private readonly DataContext _context = context;
            private readonly IMapper _mapper = mapper;

            public async Task<Result<List<LocationDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var dblocations = await _context.Locations
                    .Include(x => x.Products)
                    .ToListAsync(cancellationToken: cancellationToken);

                var location = await _context.Locations
                    .Include(x => x.Products)
                    .ProjectTo<LocationDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken: cancellationToken);

                if (location.Count < 0) return null;

                return Result<List<LocationDto>>.Success(location);

            }
        }
    }
}
