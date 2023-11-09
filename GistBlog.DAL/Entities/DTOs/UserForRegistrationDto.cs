using System.ComponentModel.DataAnnotations;

namespace GistBlog.DAL.Entities.DTOs;

public class UserForRegistrationDto
{
    //other properties

    public string? Password { get; set; }

    public string? ClientURI { get; set; }
    [Required] public string? Fullname { get; set; }
    [Required] public string? Username { get; set; }
    [Required] public string? Email { get; set; }
    [Required] public string? PhoneNumber { get; set; }
}