using GistBlog.DAL.Enums;

namespace GistBlog.DAL.Entities.Resources;

public class PersonaResource
{
    public Guid PersonaId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? OtherNames { get; set; } = string.Empty;

    public PersonaType PersonaType { get; set; }

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }
}
