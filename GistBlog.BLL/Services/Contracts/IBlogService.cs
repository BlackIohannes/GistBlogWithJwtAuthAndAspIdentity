using GistBlog.BLL.Services.Implementation;
using GistBlog.DAL.Entities.DTOs;
using GistBlog.DAL.Entities.Responses;
using Microsoft.AspNetCore.Http;

namespace GistBlog.BLL.Services.Contracts;

public interface IBlogService
{
    Task<PaginatedList<BlogDto>> GetAllBlogsAsync(int pageIndex, int pageSize);
    Task<PaginatedList<BlogDto>> GetAllUserBlogsAsync(string id, int pageIndex, int pageSize);
    Task<BlogResult> AddBlogAsync(BlogDto blogDto);
    Task<BlogDto> GetBlogByIdAsync(Guid id);
    Task<bool> UpdateBlogAsync(UpdateBlogDto blogDto);
    Task<BlogResult> DeleteBlogAsync(Guid id);
    Task<Status> UploadBlogImagesAsync(string id, IFormFile file);
    Task<IEnumerable<BlogDto>> GetAllBlogsIncludingDeletedBlogs();
}
