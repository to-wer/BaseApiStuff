﻿namespace BaseApiStuff.Models;

public class PagedResponse<T>
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }

    public int Page { get; set; }
    public int PageSize { get; set; }
}