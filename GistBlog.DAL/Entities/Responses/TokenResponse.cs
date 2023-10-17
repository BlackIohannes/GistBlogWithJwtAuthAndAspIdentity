namespace GistBlog.DAL.Entities.Responses;

public class TokenResponse
{
    public string? TokenString { get; set; }
    public DateTime ValidTo { get; set; }
    public string? UserId { get; set; }
}
