using GistBlog.DAL.Entities.DTOs;
using GistBlog.DAL.Entities.Responses;

namespace GistBlog.BLL.Services.Contracts;

public interface IBlogService
{
    Task<IQueryable<BlogDto>> GetAllUserBlogs(string id);
    Task<BlogResult> AddBlogAsync(BlogDto blogDto);
    Task<BlogDto> GetBlogByIdAsync(Guid id);
    Task<bool> UpdateBlogAsync(UpdateBlogDto blogDto);
    Task<BlogResult> DeleteBlogAsync(Guid id);
}
