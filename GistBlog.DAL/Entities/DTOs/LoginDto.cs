using System.ComponentModel.DataAnnotations;

namespace GistBlog.DAL.Entities.DTOs;

public class LoginDto
{
    [Required] public string? Username { get; set; }
    [Required] public string? Password { get; set; }
}