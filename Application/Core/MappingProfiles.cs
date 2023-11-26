namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductDto>()
             .ForMember(d => d.LocationName, o => o.MapFrom(s => s.Location));
        CreateMap<Location, LocationDto>();

        CreateMap<Location, CommonDto>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.LocationName));

        //CreateMap<SubCategory, SubCategoryDto>();

        //CreateMap<SubCategory, SubCategoryBookListDto>().ReverseMap();

        //CreateMap<SubCategory, BookStoreDto>().ReverseMap();
        //CreateMap<Location, BookStoreDto>();
        //CreateMap<LocationDto, Location>().ReverseMap();

        //CreateMap<BookDetail, BookDetailDto>()
        //   .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.SubCategory.Category.Name))
        //   .ForMember(d => d.SubCategoryName, o => o.MapFrom(s => s.SubCategory.Name))
        //   .ForMember(d => d.Author, o => o.MapFrom(s => s.Author.Name))
        //   .ForMember(d => d.Series, o => o.MapFrom(s => s.Series.Name));

        //CreateMap<BookCopies, BookCopiesDto>()
        //    .ForMember(d => d.LocationName, o => o.MapFrom(s => s.Location.Name))
        //    .ForMember(d => d.BookTitle, o => o.MapFrom(s => s.BookDetail.Title))
        //    .ForMember(d => d.TotalCopies,
        //    o => o.MapFrom(s => s.BookCopiesHistory.Where(x=>x.ActionType==ActionTypeEnum.Entered).Sum(x=>x.Copies)
        //    -s.BookCopiesHistory.Where(x=>x.ActionType==ActionTypeEnum.SoldOut || x.ActionType == ActionTypeEnum.Defective).Sum(x=>x.Copies)));

        //CreateMap<BookCopiesHistory, BookCopiesHistoryDto>();
    }
}