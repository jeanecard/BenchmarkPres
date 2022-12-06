using Mapster;
using Fakker.DAO;
using Fakker.DTO;

namespace AutomapperVsMapster.NestedTypeMapping;
public class MapsterNestedTypeMapping
{
    public static NestedUserDto Map(NestedUser source)
    {
        var destination = source.Adapt<NestedUserDto>();

        return destination;
    }
}
