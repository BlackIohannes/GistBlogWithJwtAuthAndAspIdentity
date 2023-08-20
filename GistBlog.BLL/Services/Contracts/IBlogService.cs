using GistBlog.DAL.Entities.DTOs;
using GistBlog.DAL.Entities.Responses;
using Microsoft.AspNetCore.Http;

namespace GistBlog.BLL.Services.Contracts;

public interface IBlogService
{
    Task<IEnumerable<BlogDto>> GetAllBlogsAsync();
    Task<IEnumerable<BlogDto>> GetAllUserBlogsAsync(string id);
    Task<BlogResult> AddBlogAsync(BlogDto blogDto);
    Task<BlogDto> GetBlogByIdAsync(Guid id);
    Task<bool> UpdateBlogAsync(UpdateBlogDto blogDto);
    Task<BlogResult> DeleteBlogAsync(Guid id);
    Task<Status> UploadBlogImagesAsync(string id, IFormFile file);
    Task<IEnumerable<BlogDto>> GetAllBlogsIncludingDeletedBlogs();
}
