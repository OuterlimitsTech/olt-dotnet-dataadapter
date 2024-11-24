using OLT.DataAdapters.Tests.Assets.Models;

namespace OLT.DataAdapters.Tests.PagedAdapterTests.Models
{
    public class PagedAdapterObject1 : PersonOne
    {
        public static PagedAdapterObject1 FakerData()
        {
            var result = new PagedAdapterObject1
            {
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last(),
            };
            return result;
        }

        public static List<PagedAdapterObject1> FakerList(int number)
        {
            var list = new List<PagedAdapterObject1>();
            for (int i = 0; i < number; i++)
            {
                var item = FakerData();
                list.Add(item);
            }
            return list;
        }
    }
}
