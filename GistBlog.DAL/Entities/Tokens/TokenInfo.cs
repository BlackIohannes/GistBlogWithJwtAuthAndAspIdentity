namespace GistBlog.DAL.Entities.Tokens;

public class TokenInfo
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryDate { get; set; }
}