using GistBlog.DAL.Entities.DTOs.socials;
using Microsoft.AspNetCore.Mvc;

namespace GistBlog.BLL.Services.Contracts;

public interface IGoogleAuthService
{
    Task<IActionResult> ExternalLogin(GoogleAuthDto googleAuth);
}
