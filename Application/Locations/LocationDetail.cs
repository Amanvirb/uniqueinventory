namespace Application.Locations;
public class LocationDetail
{
    public class Query : IRequest<Result<LocationDto>>
    {
        public string Name { get; set; }
    }
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<LocationDto>>
    {
        private readonly DataContext _context = context;
        private readonly IMapper _mapper = mapper;


        public async Task<Result<LocationDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var location = await _context.Locations
                .Include(x => x.Products)
                .ProjectTo<LocationDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken: cancellationToken);

            if (location is null) return null;

            return Result<LocationDto>.Success(location);

        }
    }

}
