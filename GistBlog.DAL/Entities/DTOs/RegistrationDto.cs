using System.ComponentModel.DataAnnotations;

namespace GistBlog.DAL.Entities.DTOs;

public class RegistrationDto
{
    [Required] public string? Fullname { get; set; }
    [Required] public string? Username { get; set; }
    [Required] public string? Email { get; set; }
    [Required] public string? PhoneNumber { get; set; }
    [Required] public string? Password { get; set; }
}