namespace GistBlog.DAL.Entities.Models;

public class BaseModel
{
    public Guid Id { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public DateTime? DateUpdated { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime DateDeleted { get; set; } = DateTime.Now;
}
