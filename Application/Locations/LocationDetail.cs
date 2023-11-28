
using SQLitePCL;
using System.Linq;

namespace Application.Locations;
public class LocationDetail
{
    public class Query : IRequest<Result<LocationDto>>
    {
        public int Id { get; set; }
        public string PartNumberName { get; set; }
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
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

            if (location is null) return null;

            var partNumber = location.Products.Where(x => x.PartNumberName.Contains(request.PartNumberName));

            var partNumberCount = location.Products.Where(x => x.PartNumberName.Contains(request.PartNumberName)).Count();

            var emptySpace = location.TotalCapacity - location.Products.Count;

           
            location = new LocationDto
            {
                Name = location.Name,
                TotalCapacity = location.TotalCapacity,
                TotalProducts = location.Products.Count,
                CountOfEnteredPartNumberProducts = partNumberCount,
                EmptySpace = emptySpace,
                Message = emptySpace < location.TotalCapacity
                ? $"You can place {emptySpace} Products of {request.PartNumberName} part number" : "Space is full",
                Products = location.Products
            };

            return Result<LocationDto>.Success(location);

        }
    }

}
