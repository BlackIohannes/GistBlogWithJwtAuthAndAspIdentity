using System.ComponentModel.DataAnnotations.Schema;
using GistBlog.DAL.Entities.Models.Domain;
using GistBlog.DAL.Enums;

namespace GistBlog.DAL.Entities.Models;

public class Blog : BaseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }

    [ForeignKey("AppUser")] public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}