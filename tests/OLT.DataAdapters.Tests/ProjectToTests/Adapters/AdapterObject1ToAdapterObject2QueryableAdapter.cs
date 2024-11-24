using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using OLT.DataAdapters.Tests.ProjectToTests.Models;

namespace OLT.DataAdapters.Tests.ProjectToTests.Adapters;

public class AdapterObject1ToAdapterObject2QueryableAdapter : OltAdapterQueryable<QueryableAdapterObject1, QueryableAdapterObject2>
{
    public AdapterObject1ToAdapterObject2QueryableAdapter()
    {
        this.WithOrderBy(p => p.OrderBy(o => o.FirstName).ThenBy(o => o.LastName));
    }

    public override IQueryable<QueryableAdapterObject2> Map(IQueryable<QueryableAdapterObject1> queryable)
    {
        return queryable.Select(entity => new QueryableAdapterObject2
        {
            Name = new PersonName
            {
                First = entity.FirstName,
                Last = entity.LastName,
            }
        });
    }

    public override void Map(QueryableAdapterObject1 source, QueryableAdapterObject2 destination)
    {
        throw new NotImplementedException();
    }

    public override void Map(QueryableAdapterObject2 source, QueryableAdapterObject1 destination)
    {
        throw new NotImplementedException();
    }
}

