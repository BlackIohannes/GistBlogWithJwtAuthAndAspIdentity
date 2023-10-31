namespace GistBlog.DAL.Entities.Resources;

public class ListResource<T> : StatusResource
{
    public IEnumerable<T>? Data { get; set; }
    public long? Count { get; set; }
}
