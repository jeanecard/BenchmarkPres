using AutoMapper;
using Fakker.DAO;
using Fakker.DTO;

namespace AutomapperVsMapster.NestedTypeMapping;
public class AutoMapperNestedTypeMapping
{
    public static IMapper Mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<NestedUser, NestedUserDto>();
            cfg.CreateMap<NestedAddress, NestedAddressDto>();
        })
        .CreateMapper();

    public static NestedUserDto Map(NestedUser source)
    {
        var destination = Mapper.Map<NestedUserDto>(source);

        return destination;
    }
}
