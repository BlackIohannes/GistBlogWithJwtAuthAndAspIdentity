using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Entities.DTOs;
using GistBlog.DAL.Entities.Models.UserEntities;
using GistBlog.DAL.Entities.Resources;
using GistBlog.DAL.Enums;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GistBlog.API.Controllers;

[ApiController]
[Route("api/v1/")]
public class AuthorizationController : BaseController
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<AuthorizationController> _logger;

    public AuthorizationController(IAuthenticationService authenticationService, ILogger<AuthorizationController> logger) : base(logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    [SwaggerOperation(Summary = "Register")]
    [HttpPost("user-registration")]
    public async Task<IActionResult> Register([FromBody] RegistrationDto model)
    {
        var status = await _authenticationService.SignupAsync(model);

        if (status == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return CreatedAtAction(nameof(Register), status);
    }

    [SwaggerOperation(Summary = "Login")]
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDto model)
    {
        var userLogin = await _authenticationService.LoginAsync(model);

        if (userLogin == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(userLogin);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(string username)
    {
        var loggedOutUser = await _authenticationService.LogoutAsync(username);

        if (loggedOutUser == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(loggedOutUser);
    }

    [HttpPost("login-status")]
    public async Task<IActionResult> LoginStatusAsync(string username)
    {
        var loginStatus = await _authenticationService.LoginStatusAsync(username);

        if (loginStatus == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(loginStatus);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordDto model)
    {
        var forgotPassword = await _authenticationService.ForgotPasswordAsync(model);

        if (forgotPassword == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(forgotPassword);
    }

    [SwaggerOperation(Summary = "ChangePassword")]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
    {
        var changePassword = await _authenticationService.ChangePasswordAsync(model);

        if (changePassword == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(changePassword);
    }

    [SwaggerOperation(Summary = "AdminRegistration")]
    [HttpPost("admin-registration")]
    public async Task<IActionResult> AdminRegistration([FromBody] RegistrationDto model)
    {
        var newAdminRegistration = await _authenticationService.AdminRegistrationAsync(model);

        if (newAdminRegistration == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return CreatedAtAction(nameof(AdminRegistration), newAdminRegistration);
    }

    [HttpPost("create-roles")]
    public async Task<IActionResult> CreateRoles([FromBody] List<string> roles)
    {
        var createdRoles = await _authenticationService.CreateRolesAsync(roles);

        if (createdRoles is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(createdRoles);
    }

    [HttpPost("assign-roles")]
    public async Task<IActionResult> AssignRoles([FromBody] List<string> roles, string username)
    {
        var assignedRoles = await _authenticationService.AssignRolesAsync(roles, username);

        if (assignedRoles is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(assignedRoles);
    }

    [HttpPut("edit-role")]
    public async Task<IActionResult> EditRoleAsync([FromBody] EditRoleDto model)
    {
        var editRoleAsync = await _authenticationService.EditRoleAsync(model);

        if (editRoleAsync is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(editRoleAsync);
    }

    [HttpDelete("delete-role")]
    public async Task<IActionResult> DeleteRoleAsync(string roleName)
    {
        var deleteRoleAsync = await _authenticationService.DeleteRoleAsync(roleName);

        if (deleteRoleAsync is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(deleteRoleAsync);
    }

    [HttpPut("edit-user-role")]
    public async Task<IActionResult> EditUserRoleAsync([FromBody] EditUserRoleDto model)
    {
        var editUserRoleDto = await _authenticationService.EditUserRoleAsync(model);

        if (editUserRoleDto is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(editUserRoleDto);
    }

    [HttpDelete("delete-user-role")]
    public async Task<IActionResult> DeleteUserRoleAsync([FromBody] DeleteUserRoleDto model)
    {
        var deleteUserRoleAsync = await _authenticationService.DeleteUserRoleAsync(model);

        if (deleteUserRoleAsync is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(deleteUserRoleAsync);
    }

    [HttpGet("get-all-roles")]
    public async Task<IActionResult> GetAllRolesAsync()
    {
        var getRoles = await _authenticationService.GetAllRolesAsync();

        if (getRoles is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(getRoles);
    }

    [HttpGet("get-user-roles")]
    public async Task<IActionResult> GetUserRolesAsync(string username)
    {
        var getUserRoles = await _authenticationService.GetUserRolesAsync(username);

        if (getUserRoles is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(getUserRoles);
    }

    [HttpGet("get-all-users-and-roles")]
    public async Task<IActionResult> GetAllUsersAndRolesAsync()
    {
        var getAllUsersRoles = await _authenticationService.GetAllUsersAndRolesAsync();

        if (getAllUsersRoles is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(getAllUsersRoles);
    }

    [HttpDelete("delete-user")]
    public async Task<IActionResult> DeleteUserAsync(string username)
    {
        var deleteUser = await _authenticationService.DeleteUserAsync(username);

        if (deleteUser is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(deleteUser);
    }

    [HttpGet("get-all-users")]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var getAllUsersAsync = await _authenticationService.GetAllUsersAsync();

        if (getAllUsersAsync is null)
            return StatusCode(StatusCodes.Status404NotFound);

        return Ok(getAllUsersAsync);
    }
}
