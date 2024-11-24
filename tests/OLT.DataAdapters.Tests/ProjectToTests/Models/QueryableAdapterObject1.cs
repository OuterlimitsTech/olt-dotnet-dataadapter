using OLT.DataAdapters.Tests.Assets.Models;

namespace OLT.DataAdapters.Tests.ProjectToTests.Models;

public class QueryableAdapterObject1 : PersonOne
{
    public static QueryableAdapterObject1 FakerData()
    {
        var result = new QueryableAdapterObject1
        {
            FirstName = Faker.Name.First(),
            LastName = Faker.Name.Last(),
        };
        return result;
    }

    public static List<QueryableAdapterObject1> FakerList(int number)
    {
        var list = new List<QueryableAdapterObject1>();
        for (int i = 0; i < number; i++)
        {
            var item = FakerData();
            list.Add(item);
        }
        return list;
    }
}
