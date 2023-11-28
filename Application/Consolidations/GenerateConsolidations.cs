namespace Application.Consolidations;
public class GenerateConsolidations
{
    public class Query : IRequest<Result<List<LocationDto>>>
    {
        public int MaxUnit { get; set; }
        public string PartNumberName { get; set; }
    }
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<List<LocationDto>>>
    {
        private readonly DataContext _context = context;
        private readonly IMapper _mapper = mapper;


        public async Task<Result<List<LocationDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
          
            var location = await _context.Locations
                   .Include(x => x.Products)
                   .ProjectTo<LocationDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken: cancellationToken);
          
            if (location.Count < 0) return null;

            //var partNumber = location.Products.Where(x => x.PartNumberName.Contains(request.PartNumberName));

            //var partNumberCount = location.Products.Where(x => x.PartNumberName.Contains(request.PartNumberName)).Count();

            //var emptySpace = location.TotalCapacity - location.Products.Count;


            //location = new LocationDto
            //{
            //    Name = location.Name,
            //    TotalCapacity = location.TotalCapacity,
            //    TotalProducts = location.Products.Count,
            //    CountOfEnteredPartNumberProducts = partNumberCount,
            //    EmptySpace = emptySpace,
            //    Message = emptySpace < location.TotalCapacity
            //    ? $"You can place {emptySpace} Products of {request.PartNumberName} part number" : "Space is full",
            //    Products = location.Products
            //};

            return Result<List<LocationDto>>.Success(location);

        }
    }

}
