using Application.Orders;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductDto>()
             .ForMember(d => d.LocationName, o => o.MapFrom(s => s.Location.Name))
             .ForMember(d => d.ProductNumberName, o => o.MapFrom(s => s.ProductNumber.Name));

        CreateMap<Location, LocationDto>();

        CreateMap<Location, CommonDto>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name));

        CreateMap<Location, ConsolidationPickDto>();
        CreateMap<ProductNumber, ProductNumberDto>();

        CreateMap<ProductUpdateHistory, ProductUpdateHistoryDto>();
        CreateMap<SerialNumberHistory, SerialNumberHistoryDto>();

        CreateMap<Order, CreateOrderDto>();

        CreateMap<Order, FullOrderDetailDto>()
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.AppUser.UserName));

        CreateMap<OrderDetail, OrderDetailDto>()
            .ForMember(d => d.OrderedProductNumber, o => o.MapFrom(s => s.ProductNumber.Name));
    }
}