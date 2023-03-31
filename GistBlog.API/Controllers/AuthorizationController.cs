using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GistBlog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthorizationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("UserRegistration")]
    public async Task<IActionResult> Register([FromBody] RegistrationDto model)
    {
        var newUserRegistration = await _authenticationService.Register(model);

        if (newUserRegistration == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(newUserRegistration);
    }

    [HttpPost("Login")]
    public async Task<ActionResult> Login([FromBody] LoginDto model)
    {
        var userLogin = await _authenticationService.Login(model);

        if (userLogin == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(userLogin);
    }

    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
    {
        var changePassword = await _authenticationService.ChangePassword(model);

        if (changePassword == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(changePassword);
    }

    [HttpPost("AdminRegistration")]
    public async Task<IActionResult> AdminRegistration([FromBody] RegistrationDto model)
    {
        var newAdminRegistration = await _authenticationService.AdminRegistration(model);

        if (newAdminRegistration == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(newAdminRegistration);
    }
}
