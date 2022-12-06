using Bogus;
using Fakker.DAO;
using Fakker.DTO;

namespace AutomapperVsMapster.NestedTypeMapping;
public class NestedTypeMappingDataGenerator
{
    public static List<NestedUser> GetSources(int count = 1000)
    {
        var faker = new Faker<NestedUser>()
            .Rules((f, o) =>
            {
                o.Id = f.Random.Number();
                o.FirstName = f.Name.FirstName();
                o.LastName = f.Name.LastName();
                o.Email = f.Person.Email;
                o.Address = new NestedAddress()
                {
                    AddressLine1 = f.Address.BuildingNumber(),
                    AddressLine2 = f.Address.CardinalDirection(),
                    City = f.Address.City(),
                    State = f.Address.State(),
                    Country = f.Address.Country(),
                    ZipCode = f.Address.ZipCode()
                };
            });
        return faker.Generate(count);
    }
}
