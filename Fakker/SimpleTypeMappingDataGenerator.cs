using Bogus;
using Fakker.DAO;

namespace AutomapperVsMapster.SimpleTypeMapping;
public static class SimpleTypeMappingDataGenerator
{
    public static List<Fakker.DAO.User> GetSources(int count = 1000)
    {
        var faker = new Faker<User>()
            .Rules((f, o) =>
            {
                o.Id = f.Random.Number();
                o.Name = f.Name.FullName();
                o.Email = f.Person.Email;
                o.IsActive = f.Random.Bool();
                o.CreatedAt = DateTime.Now;
            });
        return faker.Generate(count);
    }
}
