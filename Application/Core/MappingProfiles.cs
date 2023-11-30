namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductDto>()
             .ForMember(d => d.LocationName, o => o.MapFrom(s => s.Location.Name))
             .ForMember(d=> d.PartNumberName, o=>o.MapFrom(s=>s.PartNumber.Name));
       
        CreateMap<Location, LocationDto>();

        CreateMap<Location, CommonDto>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name));

        CreateMap<Location, ConsolidationDto>();
        CreateMap<PartNumber, PartNumberDto>();

        CreateMap<ProductUpdateHistory, ProductUpdateHistoryDto>();
        CreateMap<SerialNumberHistory, SerialNumberHistoryDto>();
    }
}