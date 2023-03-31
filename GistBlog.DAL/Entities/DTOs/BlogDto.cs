using System.ComponentModel.DataAnnotations;
using GistBlog.DAL.Enums;

namespace GistBlog.DAL.Entities.DTOs;

public class BlogDto
{
    [Required] public string Title { get; set; }
    [Required] public string Description { get; set; }
    [Required] public Category Category { get; set; }
    [Required] public string AppUserId { get; set; }
}
