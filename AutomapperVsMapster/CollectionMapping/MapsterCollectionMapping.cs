using Fakker.DAO;
using Fakker.DTO;
using Mapster;

namespace AutomapperVsMapster.CollectionMapping;
public class MapsterCollectionMapping
{
    public static List<CollectionUserDto> Map(List<CollectionUser> source)
    {
        var destination = source.Adapt<List<CollectionUserDto>>();

        return destination;
    }
}
