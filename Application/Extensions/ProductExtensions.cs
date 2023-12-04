namespace Application.Extensions;
public static class ProductExtensions
{
    public static IQueryable<PartNumberDto> Sort(this IQueryable<PartNumberDto> query, string orderBy)
    {
        if (orderBy == null) return query;
        query = orderBy switch
        {
            "quantiyDesc" => query.OrderByDescending(p => p.Quantity),
            _ => query.OrderBy(p => p.Quantity),
        };
        return query;
    }
    public static IQueryable<PartNumberDto> Search(this IQueryable<PartNumberDto> query, string PartNumberName)
    {
        if (PartNumberName == null) return query;

        var lowerCaseSearchTerm = PartNumberName.Trim().ToUpper();

        return query.Where(p => p.Name.Contains(lowerCaseSearchTerm));

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
