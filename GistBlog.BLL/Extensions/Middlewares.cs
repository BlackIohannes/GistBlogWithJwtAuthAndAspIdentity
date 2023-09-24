using GistBlog.BLL.Services.Contracts;
using GistBlog.BLL.Services.Implementation;
using GistBlog.DAL.Configurations;
using GistBlog.DAL.Entities.Models.Domain;
using GistBlog.DAL.Repository.Contracts;
using GistBlog.DAL.Repository.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
        services.AddLogging(building =>
        {
            building.AddConsole();
            building.AddDebug();
        });
        services.AddScoped<ICommentService, CommentService>();
    }
}
