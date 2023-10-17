namespace GistBlog.DAL.Entities.DTOs;

public class CommentDto
{
    public string? Description { get; set; }
    public string? AppUserId { get; set; }
    public Guid BlogId { get; set; }
}
