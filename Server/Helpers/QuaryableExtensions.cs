using BlazorCrud.Shared.Models;

namespace BlazorCrud.Server.Helpers;

public static class QuaryableExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, Pagination pagination)
    {
        return queryable
            .Skip((pagination.Page - 1) * pagination.RowsPerPage)
            .Take(pagination.RowsPerPage);
    }
}