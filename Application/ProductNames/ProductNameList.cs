using Application.Core;
using Application.Extensions;
using Application.ProductNames.Dtoæ;
using System.Collections.Generic;

namespace Application.ProductNames;
public class ProductNameList
{
    public class Query : IRequest<Result<PagedList<AddProductNameDto>>>
    {
        public ProductNameSearchParams Params { get; set; }
    }
    public class Handler(DataContext context, IMapper mapper) : IRequestHandler<Query, Result<PagedList<AddProductNameDto>>>
    {
        private readonly DataContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<PagedList<AddProductNameDto>>> Handle(Query request, CancellationToken ct)
        {
            var output = new List<AddProductNameDto>();

            var param = request.Params;

            var query = _context.ProductNames
                 .Include(x => x.Products).ThenInclude(x => x.Location)
                 .ProjectTo<AddProductNameDto>(_mapper.ConfigurationProvider)
                 .SearchProductName(param)
                 .AsQueryable();

            //var productNames = await query.ToListAsync(ct);

            //foreach (var ProductName in productNames)
            //{
            //    output.Add(new ()
            //    {
            //        Name = ProductName.Name,
            //        Slug = ProductName.Slug,
            //        Description = ProductName.Description,
            //        Price = ProductName.Price,
            //        Tags = ProductName.Tags,
            //    });
            //}

            //var finalQuery = output.AsQueryable();

            return Result<PagedList<AddProductNameDto>>.Success(
                await PagedList<AddProductNameDto>.CreateAsync(query, request.Params.PageNumber,
                request.Params.PageSize));

            //return Result<PagedList<AddProductNameDto>>.Success(output);

        }
    }

}
