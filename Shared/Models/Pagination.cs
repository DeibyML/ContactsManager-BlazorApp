namespace BlazorCrud.Shared.Models;

public class Pagination
{
    public int Page { get; set; } = 1;
    public int RowsPerPage { get; set; } = 10;
}