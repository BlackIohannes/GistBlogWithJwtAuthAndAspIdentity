namespace GistBlog.DAL.Entities.Resources;

public class ObjectResource<T> : StatusResource
{
    public T? Data { get; set; }
}
