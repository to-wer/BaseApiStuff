using BaseApiStuff.Enums;

namespace BaseApiStuff.Models;

public class QueryParameters
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 15;

    public string SortField { get; set; } = "Id";
    public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
}