using Microsoft.AspNetCore.Identity;

namespace GistBlog.DAL.Entities.Models.Domain;

public class AppUser : IdentityUser
{
    public string? Fullname { get; set; }
    public override string? PhoneNumber { get; set; }
    public IList<Blog> Blogs { get; set; } = new List<Blog>();
}
