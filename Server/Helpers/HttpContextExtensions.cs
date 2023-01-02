using Microsoft.EntityFrameworkCore;

namespace BlazorCrud.Server.Helpers;

public static class HttpContextExtensions
{
    public static async Task InsertPaginationParametersInResponse<T>(this HttpContext httpContext, IQueryable<T> queryable, int recordsPerPage)
    {
        if (httpContext == null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        double count = await queryable.CountAsync();
        double totalPages = Math.Ceiling(count / recordsPerPage);

        httpContext.Response.Headers.Add("totalPages", totalPages.ToString());
    }
}