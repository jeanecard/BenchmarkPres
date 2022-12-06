using AutoMapper;
using Fakker.DAO;
using Fakker.DTO;

namespace AutomapperVsMapster.CollectionMapping;
public class AutoMapperCollectionMapping
{
    public static IMapper Mapper = new MapperConfiguration(cfg => cfg.CreateMap<CollectionUser, CollectionUserDto>()).CreateMapper();

    public static List<CollectionUserDto> Map(List<CollectionUser> source)
    {
        var destination = Mapper.Map<List<CollectionUserDto>>(source);

        return destination;
    }
}
