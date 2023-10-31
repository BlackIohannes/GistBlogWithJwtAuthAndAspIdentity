using GistBlog.DAL.Entities.Models.UserEntities;
using GistBlog.DAL.Entities.Resources;

namespace GistBlog.BLL.Services;

public interface IAuthService
{
    Task<ObjectResource<string>> LoginAsync(string email);

    Task<ObjectResource<string>> LoginAsync(BasicAuthPayload payload);
}
