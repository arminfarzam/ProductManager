using ProductManager.Application.DTOs.Paging;

namespace ProductManager.Application.Utilities.Paging;

public static class PagingExtensions
{
    public static IQueryable<T> Paging<T>(this IQueryable<T> queryable, BasePaging pager)
    {
        return queryable.Skip(pager.SkipSize).Take(pager.PageSize);
    }
}