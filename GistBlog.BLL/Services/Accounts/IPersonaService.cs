using GistBlog.DAL.Entities.Models.Payload;
using GistBlog.DAL.Entities.Resources;

namespace GistBlog.BLL.Services.Accounts;

public interface IPersonaService
{
    Task<ObjectResource<string>> CreateAsync(CreatePersonaPayload payload);

    Task<StatusResource> AssignNewRoleAsync(AssignNewRolePayload payload);

    Task<ObjectResource<PersonaResource>> GetUserByIdAsync(Guid userId);

    Task<ListResource<PersonaResource>> GetUsersAsync(FilterPersonaPayload payload, int skip, int? take);
}
