using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Configurations;
using GistBlog.DAL.Configurations.EmailConfig.messages;
using GistBlog.DAL.Configurations.EmailConfig.services;
using GistBlog.DAL.Entities.DTOs;
using GistBlog.DAL.Entities.Models.UserEntities;
using GistBlog.DAL.Entities.Responses;
using GistBlog.DAL.Entities.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GistBlog.BLL.Services.Implementation;

public class AuthenticationService : IAuthenticationService
{
    private readonly DataContext _context;
    private readonly IEmailSender _emailSender;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;

    public AuthenticationService(
        DataContext context,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ITokenService tokenService,
        IEmailSender emailSender
    )
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
        _emailSender = emailSender;
    }

    public async Task<Status> SignupAsync(RegistrationDto model, string scheme)
    {
        var status = new Status();
        if (string.IsNullOrEmpty(model.Fullname) || string.IsNullOrEmpty(model.Username) ||
            string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.PhoneNumber) ||
            string.IsNullOrEmpty(model.Password))
        {
            status.StatusCode = 0;
            status.Message = "Please fill in all the required fields.";
            return status;
        }

        // Check if user exists
        var userExist = await _userManager.FindByNameAsync(model.Username);
        if (userExist != null)
        {
            status.StatusCode = 0;
            status.Message = "Username already exists.";
            return status;
        }

        var emailExist = await _userManager.FindByEmailAsync(model.Email);
        if (emailExist != null)
        {
            status.StatusCode = 0;
            status.Message = "Email already exists.";
            return status;
        }

        var user = new AppUser
        {
            UserName = model.Username,
            SecurityStamp = Guid.NewGuid().ToString(),
            Email = model.Email,
            Fullname = model.Fullname,
            PhoneNumber = model.PhoneNumber,
            EmailConfirmed = false
        };

        // Create user
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            status.StatusCode = 0;
            status.Message = "User creation failed.";
            return status;
        }
        
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        
        var param = new Dictionary<string, string?>
        {
            { "token", token },
            { "email", user.Email }
        };
        
        var callback =
            $"{scheme}://https://localhost:7154/Account/ConfirmEmail?token={token}";

        var message = new Message(new[] { user.Email }, "Confirmation email link", callback, null);
        await _emailSender.SendEmailAsync(message);

        // Add roles
        if (!await _roleManager.RoleExistsAsync(UserRole.User))
            await _roleManager.CreateAsync(new IdentityRole(UserRole.User));

        if (await _roleManager.RoleExistsAsync(UserRole.User))
            await _userManager.AddToRoleAsync(user, UserRole.User);

        status.StatusCode = 1;
        status.Message = "User successfully registered.";
        return status;
    }

    public async Task<LoginResponse> LoginAsync(LoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);

        // if (!await _userManager.IsEmailConfirmedAsync(user))
        //     throw new Exception("Unauthorized! Email is not confirmed.");
        //
        // if (!await _userManager.CheckPasswordAsync(user, model.Password))
        //     throw new Exception("Password is not valid.");
        
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new("Id", user.Id),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var userRole in userRoles) authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            var token = _tokenService.GenerateToken(authClaims);
            var refreshToken = _tokenService.GetRefreshToken();
            var tokenInfo = await _context.TokenInfos!.FirstOrDefaultAsync(x => x.Username == user.UserName);
            if (tokenInfo == null)
            {
                var info = new TokenInfo
                {
                    Username = user.UserName,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryDate = DateTime.Now.AddMinutes(10)
                };
                _context.TokenInfos!.Add(info);
            }
            else
            {
                tokenInfo.RefreshToken = refreshToken;
                tokenInfo.RefreshTokenExpiryDate = DateTime.Now.AddMinutes(30);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return new LoginResponse
            {
                Id = user.Id,
                Name = user.Fullname,
                Username = user.UserName,
                accessToken = token.TokenString,
                RefreshToken = refreshToken,
                Expiration = token.ValidTo,
                StatusCode = 1,
                Message = "Logged In"
            };
        }

        // Login failed condition
        return new LoginResponse
        {
            StatusCode = 0,
            Message = "Invalid username or password",
            accessToken = "",
            Expiration = null
        };
    }

    public async Task<Status> AdminRegistrationAsync([FromBody] RegistrationDto model)
    {
        var status = new Status();
        if (string.IsNullOrEmpty(model.Fullname) || string.IsNullOrEmpty(model.Username) ||
            string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.PhoneNumber) ||
            string.IsNullOrEmpty(model.Password))
        {
            status.StatusCode = 0;
            status.Message = "Please pass all the required fields";
            return status;
        }

        // check if user exists
        var userExists = await _userManager.FindByNameAsync(model.Username);
        if (userExists != null)
        {
            status.StatusCode = 0;
            status.Message = "Username already exists.";
            return status;
        }

        var emailExist = await _userManager.FindByEmailAsync(model.Email);

        if (emailExist != null)
        {
            status.StatusCode = 0;
            status.Message = "Email already exists.";
            return status;
        }

        var user = new AppUser
        {
            UserName = model.Username,
            SecurityStamp = Guid.NewGuid().ToString(),
            Email = model.Email,
            Fullname = model.Fullname,
            PhoneNumber = model.PhoneNumber
        };
        // create a user here
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            status.StatusCode = 0;
            status.Message = "User creation failed";
            return status;
        }

        // add roles here
        // for admin registration UserRoles.Admin instead of UserRoles.Roles
        if (!await _roleManager.RoleExistsAsync(UserRole.Admin))
            await _roleManager.CreateAsync(new IdentityRole(UserRole.Admin));

        if (await _roleManager.RoleExistsAsync(UserRole.Admin))
            await _userManager.AddToRoleAsync(user, UserRole.Admin);

        status.StatusCode = 1;
        status.Message = "Successfully registered";
        return status;
    }

    public async Task<IdentityResult> ConfirmEmail(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            throw new Exception("Email not found!");

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result is null) throw new Exception("Email confirmation failed!");

        return result;
    }

    #region other auths

    // logout
    public async Task<Status> LogoutAsync(string username)
    {
        var status = new Status();
        var tokenInfo = await _context.TokenInfos.FirstOrDefaultAsync(x => x.Username == username);
        if (tokenInfo != null)
        {
            _context.TokenInfos.Remove(tokenInfo);
            await _context.SaveChangesAsync();
            status.StatusCode = 1;
            status.Message = "Logged out successfully";
        }
        else
        {
            status.StatusCode = 0;
            status.Message = "No user found";
        }

        return status;
    }

    // login status
    public async Task<Status> LoginStatusAsync(string username)
    {
        var status = new Status();
        var tokenInfo = await _context.TokenInfos.FirstOrDefaultAsync(x => x.Username == username);
        if (tokenInfo != null)
        {
            status.StatusCode = 1;
            status.Message = "Logged in";
        }
        else
        {
            status.StatusCode = 0;
            status.Message = "Not logged in";
        }

        return status;
    }

    public async Task<Status> ChangePasswordAsync(ChangePasswordDto model)
    {
        var status = new Status();
        // check validations
        if (string.IsNullOrEmpty(model.Username))
        {
            status.StatusCode = 0;
            status.Message = "Please pass all the valid fields";
            return status;
        }

        // lets find the user
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user is null)
        {
            status.StatusCode = 0;
            status.Message = "Invalid username";
            return status;
        }

        // check current password
        if (!await _userManager.CheckPasswordAsync(user, model.CurrentPassword))
        {
            status.StatusCode = 0;
            status.Message = "Invalid current password";
            return status;
        }

        // change password here
        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        if (!result.Succeeded)
        {
            status.StatusCode = 0;
            status.Message = "Failed to change password";
            return status;
        }

        status.StatusCode = 1;
        status.Message = "Password change was successful";

        return status;
    }

    // create roles
    public async Task<Status> CreateRolesAsync([FromBody] List<string> roles)
    {
        var status = new Status();
        if (roles == null)
        {
            status.StatusCode = 0;
            status.Message = "Please pass all the required fields";
            return status;
        }

        foreach (var role in roles)
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

        status.StatusCode = 1;
        status.Message = "Successfully created roles";
        return status;
    }

    // assign roles
    public async Task<Status> AssignRolesAsync([FromBody] List<string> roles, string username)
    {
        var status = new Status();
        if (roles == null)
        {
            status.StatusCode = 0;
            status.Message = "Please pass all the required fields";
            return status;
        }

        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            status.StatusCode = 0;
            status.Message = "Invalid username";
            return status;
        }

        foreach (var role in roles)
            if (!await _roleManager.RoleExistsAsync(role))
            {
                status.StatusCode = 0;
                status.Message = "Invalid role";
                return status;
            }

        foreach (var role in roles) await _userManager.AddToRoleAsync(user, role);

        status.StatusCode = 1;
        status.Message = "Successfully assigned roles";
        return status;
    }

    // edit role
    public async Task<Status> EditRoleAsync([FromBody] EditRoleDto model)
    {
        var status = new Status();
        if (string.IsNullOrEmpty(model.RoleName) || string.IsNullOrEmpty(model.NewRoleName))
        {
            status.StatusCode = 0;
            status.Message = "Please pass all the required fields";
            return status;
        }

        var role = await _roleManager.FindByNameAsync(model.RoleName);
        if (role is null)
        {
            status.StatusCode = 0;
            status.Message = "Invalid role";
            return status;
        }

        role.Name = model.NewRoleName;
        await _roleManager.UpdateAsync(role);

        status.StatusCode = 1;
        status.Message = "Successfully edited role";
        return status;
    }

    // delete role
    public async Task<Status> DeleteRoleAsync(string roleName)
    {
        var status = new Status();
        if (string.IsNullOrEmpty(roleName))
        {
            status.StatusCode = 0;
            status.Message = "Please pass all the required fields";
            return status;
        }

        var role = await _roleManager.FindByNameAsync(roleName);
        if (role is null)
        {
            status.StatusCode = 0;
            status.Message = "Invalid role";
            return status;
        }

        await _roleManager.DeleteAsync(role);

        status.StatusCode = 1;
        status.Message = "Successfully deleted role";
        return status;
    }

    // edit user role
    public async Task<Status> EditUserRoleAsync([FromBody] EditUserRoleDto model)
    {
        var status = new Status();
        if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.RoleName))
        {
            status.StatusCode = 0;
            status.Message = "Please pass all the required fields";
            return status;
        }

        var user = await _userManager.FindByNameAsync(model.Username);
        if (user is null)
        {
            status.StatusCode = 0;
            status.Message = "Invalid username";
            return status;
        }

        var role = await _roleManager.FindByNameAsync(model.RoleName);
        if (role is null)
        {
            status.StatusCode = 0;
            status.Message = "Invalid role";
            return status;
        }

        await _userManager.RemoveFromRoleAsync(user, role.Name);
        await _userManager.AddToRoleAsync(user, role.Name);

        status.StatusCode = 1;
        status.Message = "Successfully edited user role";
        return status;
    }

    // delete user role
    public async Task<Status> DeleteUserRoleAsync([FromBody] DeleteUserRoleDto model)
    {
        var status = new Status();
        if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.RoleName))
        {
            status.StatusCode = 0;
            status.Message = "Please pass all the required fields";
            return status;
        }

        var user = await _userManager.FindByNameAsync(model.Username);
        if (user is null)
        {
            status.StatusCode = 0;
            status.Message = "Invalid username";
            return status;
        }

        var role = await _roleManager.FindByNameAsync(model.RoleName);
        if (role is null)
        {
            status.StatusCode = 0;
            status.Message = "Invalid role";
            return status;
        }

        await _userManager.RemoveFromRoleAsync(user, role.Name);

        status.StatusCode = 1;
        status.Message = "Successfully deleted user role";
        return status;
    }

    // get all roles and permissions
    public async Task<List<string>?> GetAllRolesAsync()
    {
        var status = new Status();
        var roles = await _roleManager.Roles.ToListAsync();

        if (roles is null)
        {
            status.StatusCode = 0;
            status.Message = "Not found!";
            return null;
        }

        return roles.Select(x => x.Name).ToList();
    }

    // get user and specific role
    public async Task<List<string>?> GetUserRolesAsync(string username)
    {
        var status = new Status();
        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
        {
            status.StatusCode = 0;
            status.Message = "Not found!";
            return null;
        }

        return (List<string>)await _userManager.GetRolesAsync(user);
    }

    // get all users and specific roles
    public async Task<List<string>?> GetAllUsersAndRolesAsync()
    {
        var status = new Status();
        var users = await _userManager.Users.ToListAsync();

        if (users is null)
        {
            status.StatusCode = 0;
            status.Message = "Not found!";
            return null;
        }

        var result = new List<string>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            result.Add($"{user.UserName} - {string.Join(", ", roles)}");
        }

        return result;
    }

    // delete user
    public async Task<Status> DeleteUserAsync(string username)
    {
        var status = new Status();
        if (string.IsNullOrEmpty(username))
        {
            status.StatusCode = 0;
            status.Message = "Please pass all the required fields";
            return status;
        }

        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            status.StatusCode = 0;
            status.Message = "Invalid username";
            return status;
        }

        await _userManager.DeleteAsync(user);

        status.StatusCode = 1;
        status.Message = "Successfully deleted user";
        return status;
    }

    // get all users and roles
    public async Task<List<string>?> GetAllUsersAsync()
    {
        var status = new Status();
        var users = await _userManager.Users.ToListAsync();

        if (users is null)
        {
            status.StatusCode = 0;
            status.Message = "Not found!";
            return null;
        }

        return users.Select(x => x.UserName).ToList();
    }

    #endregion

    #region forget password and reset password service implementation

    // Forgot password
    public async Task<string?> GeneratePasswordResetTokenAsync(ForgotPasswordResetDto resetDto)
    {
        var user = await _userManager.FindByEmailAsync(resetDto.Email);
        return user != null ? await _userManager.GeneratePasswordResetTokenAsync(user) : null;
    }

    public async Task<bool> SendForgotPasswordEmailAsync(ForgotPasswordResetDto resetDto)
    {
        var user = await _userManager.FindByEmailAsync(resetDto.Email);
        if (user == null)
            return false;

        var message = new Message(new[] { resetDto.Email }, "Reset password token", resetDto.CallbackUrl, null);
        await _emailSender.SendEmailAsync(message);

        return true;
    }

    // reset password
    public async Task<bool> ResetPasswordAsync(PasswordResetInfoDto resetInfoDto)
    {
        var user = await _userManager.FindByEmailAsync(resetInfoDto.Email);
        if (user == null)
            return false;

        var resetPassResult = await _userManager.ResetPasswordAsync(user, resetInfoDto.Token, resetInfoDto.NewPassword);
        return resetPassResult.Succeeded;
    }

    #endregion
}