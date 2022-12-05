using Fakker.DTO;
using Fakker.DAO;


namespace AutomapperVsMapster.SimpleTypeMapping
{
    public class OldSchoolSimpleTypeMapping
    {
        public static UserDto? Map(User source)
        {
            if (source == null) return null;
            return new()
            {
                Id = source.Id,
                CreatedAt = source.CreatedAt.ToString(), 
                Email = source.Email,
                IsActive = source.IsActive,
                Name = source.Name  
            };
        }
    }
}
