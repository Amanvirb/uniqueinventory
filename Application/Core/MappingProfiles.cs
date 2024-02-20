using Application.Orders;
using Application.ProductNames.Dtoæ;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductDto>()
             .ForMember(d => d.LocationName, o => o.MapFrom(s => s.Location.Name))
             .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ProductName.Name));

        CreateMap<Location, LocationDto>();
        CreateMap<ProductName, AddProductNameDto>();

        CreateMap<Location, CommonDto>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name));

        CreateMap<Location, ConsolidationPickDto>();
        CreateMap<ProductName, ProductNameDto>();

        CreateMap<ProductUpdateHistory, ProductUpdateHistoryDto>();
        CreateMap<SerialNumberHistory, SerialNumberHistoryDto>();

        CreateMap<Order, CreateOrderDto>();

        CreateMap<Order, FullOrderDetailDto>()
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.AppUser.UserName));

        CreateMap<OrderDetail, OrderDetailDto>()
            .ForMember(d => d.OrderedProductName, o => o.MapFrom(s => s.ProductName.Name));
    }
}