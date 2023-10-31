using System.ComponentModel.DataAnnotations;
using GistBlog.DAL.Enums;
using GistBlog.DAL.Infrastructures;

namespace GistBlog.DAL.Entities.Models.Validations;

public class ProfileTypeAttribute : ValidationAttribute
{
    private readonly PersonaType? blacklistedPersonaType;
    private readonly PersonaType expectedPersonaType;

    public ProfileTypeAttribute()
    {
        expectedPersonaType = (PersonaType)EnumExtensions.All<PersonaType>();
        blacklistedPersonaType = null;
    }

    public ProfileTypeAttribute(PersonaType expectedPersonaType)
    {
        this.expectedPersonaType = expectedPersonaType;
        blacklistedPersonaType = null;
    }

    public ProfileTypeAttribute(PersonaType expectedPersonaType, PersonaType blacklistedPersonaType)
    {
        this.expectedPersonaType = expectedPersonaType;
        this.blacklistedPersonaType = blacklistedPersonaType;
    }

    public override bool IsValid(object? value)
    {
        if (value is not PersonaType specifiedPersonaType) return false;

        if (!blacklistedPersonaType.HasValue || (blacklistedPersonaType & specifiedPersonaType) == 0)
            return (specifiedPersonaType & expectedPersonaType) > 0;

        ErrorMessage = $"{specifiedPersonaType.ToPascalString()} Roles Are Not Allowed";

        return false;
    }
}
