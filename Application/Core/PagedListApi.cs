namespace Application.Core;

public class PagedListApi<T> : List<T>
{
    public PagedListApi(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        PageSize = pageSize;
        TotalCount = count;
        AddRange(items);
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public async static Task<PagedListApi<T>> CreateAsync(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var listSource = source.ToList(); // Pretvaranje u listu

        var count = listSource.Count;
        var items = listSource.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        return new PagedListApi<T>(items, count, pageNumber, pageSize);
    }

}