using System.ComponentModel.DataAnnotations.Schema;
using GistBlog.DAL.Entities.Models.UserEntities;

namespace GistBlog.DAL.Entities.Models;

public class Comment : BaseModel
{
    public string? Description { get; set; }

    public Guid BlogId { get; set; }
    [ForeignKey(nameof(BlogId))] public Blog Blog { get; set; } = null!;

    public string? AppUserId { get; set; }
    [ForeignKey(nameof(AppUserId))] public AppUser AppUser { get; set; } = null!;
}
