namespace OLT.Core;

[Obsolete("Removing in v10.x - Move to OltBeforeMapOrderBy")]
public interface IOltAdapterPaged<TEntity, TDestination> : IOltAdapterQueryable<TEntity, TDestination>
{
    IOrderedQueryable<TEntity> DefaultOrderBy(IQueryable<TEntity> queryable);
}