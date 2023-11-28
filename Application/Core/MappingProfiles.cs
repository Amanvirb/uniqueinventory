namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductDto>()
             .ForMember(d => d.LocationName, o => o.MapFrom(s => s.Location.LocationName))
             .ForMember(d=> d.PartNumberName, o=>o.MapFrom(s=>s.PartNumberName.PartNumberName));
       
        CreateMap<Location, LocationDto>();

        CreateMap<Location, CommonDto>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.LocationName));

        CreateMap<ProductUpdateHistory, ProductUpdateHistoryDto>();
        CreateMap<SerialNumberHistory, SerialNumberHistoryDto>();
    }
}