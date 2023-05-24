using GistBlog.BLL.Services.Contracts;
using GistBlog.BLL.Services.Implementation;
using GistBlog.DAL.Configurations;
using GistBlog.DAL.Entities.Models.Domain;
using GistBlog.DAL.Repository.Contracts;
using GistBlog.DAL.Repository.Implementations;
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
        services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = "53306076231-4jrvd3b85ue9dk7otluf5hrtq6keu9fv.apps.googleusercontent.com";
                options.ClientSecret = "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";
            });
    }
}
