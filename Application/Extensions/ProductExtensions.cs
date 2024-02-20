using Application.ProductNames.Dtoæ;

namespace Application.Extensions;
public static class ProductExtensions
{
    public static IQueryable<ProductNameDto> Sort(this IQueryable<ProductNameDto> query, string orderBy)
    {
        if (orderBy == null) return query;
        query = orderBy switch
        {
            "quantiyDesc" => query.OrderByDescending(p => p.Quantity),
            _ => query.OrderBy(p => p.Quantity),
        };
        return query;
    }

    public static IQueryable<Product> Search(this IQueryable<Product> query, ProductSearchParams Params)
    {
        if (!string.IsNullOrEmpty(Params.SerialNo))
        {
            query = query.Where(p => p.SerialNumber == Params.SerialNo.Trim().ToUpper());
            return query;
        }
        if (!string.IsNullOrEmpty(Params.ProductName) && !string.IsNullOrEmpty(Params.Location))
        {
            query = query.Where(p => p.ProductName.Name.Contains(Params.ProductName)
                      && p.Location.Name == Params.Location);
            return query;
        }
        if (!string.IsNullOrEmpty(Params.ProductName))
            query = query.Where(p => p.ProductName.Name.Contains(Params.ProductName));

        if (!string.IsNullOrEmpty(Params.Location))
            query = query.Where(p => p.Location.Name == Params.Location);

        return query;
    }
    public static IQueryable<AddProductNameDto> SearchProductName(this IQueryable<AddProductNameDto> query, ProductNameSearchParams Params)
    {
        if (!string.IsNullOrEmpty(Params.ProductName))
        {
            query = query.Where(p => p.Name.Contains(Params.ProductName));
            return query;
        }

        return query;
    }


    //public static IQueryable<BookDetailDto> Filter(this IQueryable<BookDetailDto> query, string brands, string types)
    //{
    //    var brandList = new List<string>();
    //    var typeList = new List<string>();

    //    if (!string.IsNullOrEmpty(brands))
    //    {
    //        brandList.AddRange(brands.ToLower().Split(',').ToList());
    //    }

    //    if (!string.IsNullOrEmpty(types))
    //    {
    //        typeList.AddRange(types.ToLower().Split(',').ToList());
    //    }

    //    query = query.Where(p => brandList.Count == 0 || brandList.Contains(p.Brand.ToLower()));
    //    query = query.Where(p => typeList.Count == 0 || brandList.Contains(p.Type.ToLower()));

    //    return query;
    //}

}
