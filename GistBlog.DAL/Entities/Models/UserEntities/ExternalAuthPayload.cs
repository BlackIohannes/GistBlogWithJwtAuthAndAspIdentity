using System.ComponentModel.DataAnnotations;

namespace GistBlog.DAL.Entities.Models.UserEntities;

public class ExternalAuthPayload
{
    public string AccessToken { get; set; } = null!;
}

public class BasicAuthPayload
{
    [Required(ErrorMessage = "Please Provide Email Address")]
    [RegularExpression(StringConstants.EmailRegex, ErrorMessage = "Provide A Valid Email Address")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Please Provide A Password")]
    public string Password { get; set; } = null!;
}
