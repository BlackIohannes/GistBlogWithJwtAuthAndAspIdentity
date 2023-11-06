using GistBlog.BLL.Services.Contracts;
using GistBlog.BLL.Services.Implementation;
using GistBlog.BLL.Services.Implementation.social;
using GistBlog.DAL.Configurations;
using GistBlog.DAL.Configurations.EmailConfig.services;
using GistBlog.DAL.Entities.Models.UserEntities;
using GistBlog.DAL.Repository.Contracts;
using GistBlog.DAL.Repository.Implementations;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GistBlog.BLL.Extensions;

public static class Middlewares
{
    public static void UserRegistrationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<IUnitOfWork, UnitOfWork<DataContext>>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<EmailSettings>();
        services.Configure<MailjetSettings>(configuration.GetSection("MailjetSettings"));
        services.AddScoped<IMailjetService, MailjetService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddGoogle(options =>
                {
                    options.ClientId = configuration["Google:ClientId"];
                    options.ClientSecret = configuration["Google:ClientSecret"];
                }
            );
        services.AddScoped<IGoogleAuthService, GoogleAuthService>();
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IPasswordResetService, PasswordResetService>();
        services.AddScoped<IResetPasswordService, ResetPasswordService>();
    }
}