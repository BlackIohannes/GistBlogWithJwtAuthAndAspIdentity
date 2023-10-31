using System.ComponentModel.DataAnnotations.Schema;
using GistBlog.DAL.Entities.Models.UserEntities;

namespace GistBlog.DAL.Entities.Models;

public class ProductTransaction : BaseModel
{
    public string? Name { get; set; }
    public int Amount { get; set; }
    public string TrxnRef { get; set; }
    public string Email { get; set; }
    public bool Status { get; set; }

    [ForeignKey(nameof(AppUserId))] public Guid AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}