using System.ComponentModel.DataAnnotations;
using GistBlog.DAL.Entities.Models.UserEntities;
using GistBlog.DAL.Entities.Models.Validations;
using GistBlog.DAL.Enums;

namespace GistBlog.DAL.Entities.Models.Payload;

public class CreatePersonaPayload
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? OtherNames { get; set; } = string.Empty;

    [EnumDataType(typeof(PersonaType), ErrorMessage = "Please Provide A Valid Persona Type")]
    [ProfileType(PersonaType.Client | PersonaType.Influenzer, PersonaType.Admin)]
    public PersonaType PersonaType { get; set; } = PersonaType.Client;

    [RegularExpression(StringConstants.EmailRegex, ErrorMessage = "Please Provide A Valid Email Address")]
    public string Email { get; set; } = null!;

    [RegularExpression(StringConstants.PhoneNumberRegex,
        ErrorMessage = "Please Provide A Valid Phone Number In Int'l format")]
    public string? PhoneNumber { get; set; }

    public string? PictureUrl { get; set; }

    [Required(ErrorMessage = "Please Provide A Password")]
    public string Password { get; set; } = null!;

    [Compare(nameof(Password), ErrorMessage = "Password Do Not Match")]
    public string ConfirmPassword { get; set; } = null!;
}

public class AssignNewRolePayload
{
    [EnumDataType(typeof(PersonaType), ErrorMessage = "Please Provide A Valid Persona Type")]
    [ProfileType(PersonaType.Client | PersonaType.Influenzer, PersonaType.Admin)]
    public PersonaType PersonaType { get; set; }
}

public class FilterPersonaPayload
{
    public PersonaType? PersonaType { get; set; }

    public string? Name { get; set; }
}
