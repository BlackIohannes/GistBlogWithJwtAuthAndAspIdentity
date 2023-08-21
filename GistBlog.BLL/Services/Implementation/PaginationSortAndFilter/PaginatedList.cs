using Microsoft.EntityFrameworkCore;

namespace GistBlog.BLL.Services.Implementation.PaginationSortAndFilter;

public class PaginatedList<T> : List<T>
{
    public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        AddRange(items);
    }

    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }

    public bool PreviousPage => PageIndex > 1;
    public bool NextPage => PageIndex < TotalPages;

    // pagination action
    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}
