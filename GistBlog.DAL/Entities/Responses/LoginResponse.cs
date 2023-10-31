using GistBlog.DAL.Entities.DTOs;

namespace GistBlog.DAL.Entities.Responses;

public class LoginResponse : Status
{
    public string Id { get; set; }
    public string? accessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? Expiration { get; set; }
    public string? Name { get; set; }
    public string? Username { get; set; }
}
