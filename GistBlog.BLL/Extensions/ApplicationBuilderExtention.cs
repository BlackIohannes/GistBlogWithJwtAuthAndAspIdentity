using GistBlog.DAL.Configurations;
using Microsoft.AspNetCore.Builder;

namespace GistBlog.BLL.Extensions;

public static class ApplicationBuilderExtention
{
    public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder) =>
        applicationBuilder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
}
