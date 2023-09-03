using System.ComponentModel.DataAnnotations;
using GistBlog.DAL.Enums;
using Microsoft.AspNetCore.Http;

namespace GistBlog.DAL.Entities.DTOs;

public class BlogDto
{
    [Required] public string? Title { get; set; }
    [Required] public string? Description { get; set; }

    [Required] public Category Category { get; set; }

    // [Required] public string? ImageUrl { get; set; }
    [Required] public string? AppUserId { get; set; }
    public IFormFile? File { get; set; }
}
