using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.DbConfig;
using GistBlog.DAL.Entities.DTOs;
using GistBlog.DAL.Entities.Models.Domain;
using GistBlog.DAL.Entities.Responses;
using GistBlog.DAL.Entities.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GistBlog.BLL.Services.Implementation;

public class AuthenticationService : IAuthenticationService
{
    private readonly DataContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ITokenService _tokenService;

    public AuthenticationService(
        DataContext context,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ITokenService tokenService
    )
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
    }

    public async Task<Status> Register(RegistrationDto model)
    {
        var status = new Status();
        if (string.IsNullOrEmpty(model.Fullname) || string.IsNullOrEmpty(model.Username) ||
            string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.PhoneNumber) ||
            string.IsNullOrEmpty(model.Password))
        {
            status.StatusCode = 0;
            status.Message = "Please pass all the required fields.";

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

        var user = new AppUser()
        {
            UserName = model.Username,
            SecurityStamp = Guid.NewGuid().ToString(),
            Email = model.Email,
            Fullname = model.Fullname,
            PhoneNumber = model.PhoneNumber
        };

        // Create user here
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            status.StatusCode = 0;
            status.Message = "User creation failed";
            return status;
        }

        // Add roles
        // For admin registration, make use of UserRole.Admin instead of UserRole.User
        if (!await _roleManager.RoleExistsAsync(UserRole.User))
            await _roleManager.CreateAsync(new IdentityRole(UserRole.User));

        if (await _roleManager.RoleExistsAsync(UserRole.User))
            await _userManager.AddToRoleAsync(user, UserRole.User);

        status.StatusCode = 1;
        status.Message = "Successfully registered";

        return status;
    }

    public async Task<LoginResponse> Login(LoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _tokenService.GetToken(authClaims);
            var refreshToken = _tokenService.GetRefreshToken();
            var tokenInfo = await _context.TokenInfos.FirstOrDefaultAsync(x => x.Username == user.UserName);
            if (tokenInfo == null)
            {
                var info = new TokenInfo()
                {
                    Username = user.UserName,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryDate = DateTime.Now.AddMinutes(10)
                };
                _context.TokenInfos.Add(info);
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

            return new LoginResponse()
            {
                Name = user.Fullname,
                Username = user.UserName,
                Token = token.TokenString,
                RefreshToken = refreshToken,
                Expiration = token.ValidTo,
                StatusCode = 1,
                Message = "Logged In"
            };
        }

        // Login failed condition
        return new LoginResponse()
        {
            StatusCode = 0,
            Message = "Invalid username or password",
            Token = "",
            Expiration = null
        };
    }

    public async Task<Status> ChangePassword(ChangePasswordDto model)
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


    public async Task<Status> AdminRegistration([FromBody] RegistrationDto model)
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
        status.Message = "Sucessfully registered";
        return status;
    }
}
