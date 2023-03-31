namespace GistBlog.DAL.Entities.DTOs;

public class RefreshTokenRequestDto
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}