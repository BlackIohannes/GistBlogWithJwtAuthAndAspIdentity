using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Entities.DTOs;
using GistBlog.DAL.Entities.Models;
using GistBlog.DAL.Entities.Models.Domain;
using GistBlog.DAL.Entities.Responses;
using GistBlog.DAL.Exceptions;
using GistBlog.DAL.Repository.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace GistBlog.BLL.Services.Implementation;

public class BlogService : IBlogService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IRepository<Blog> _blogRepository;

    public BlogService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _blogRepository = _unitOfWork.GetRepository<Blog>();
        _userManager = userManager;
    }

    public async Task<IEnumerable<BlogDto>> GetAllBlogsAsync()
    {
        var blogs = await _blogRepository.GetAllAsync();

        if (blogs is null)
            throw new NotFoundException("Blogs not found.");

        return blogs.Select(x => new BlogDto()
        {
            AppUserId = x.AppUserId,
            Title = x.Title,
            Description = x.Description,
            Category = x.Category,
            ImageUrl = x.ImageUrl
        });
    }

    public async Task<IEnumerable<BlogDto>> GetAllUserBlogsAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            throw new NotFoundException("User not found.");

        var blogs = _blogRepository.GetQueryable(x => x.AppUserId == id);

        if (blogs == null)
            throw new NotFoundException("Blogs not found.");

        return blogs.Select(x => new BlogDto()
        {
            AppUserId = x.AppUserId,
            Title = x.Title,
            Description = x.Description,
            Category = x.Category,
            ImageUrl = x.ImageUrl
        });
    }

    public async Task<BlogResult> AddBlogAsync(BlogDto blogDto)
    {
        var userExist = await _userManager.FindByIdAsync(blogDto.AppUserId);

        if (userExist == null)
            throw new DAL.Exceptions.KeyNotFoundException(
                $"User Id: {blogDto.AppUserId} does not match with the post.");

        var newBlog = new Blog()
        {
            AppUserId = blogDto.AppUserId,
            Title = blogDto.Title,
            Description = blogDto.Description,
            Category = blogDto.Category,
            ImageUrl = blogDto.ImageUrl
        };

        var createdBlog = await _blogRepository.AddAsync(newBlog);

        if (createdBlog != null)

            return new BlogResult()
            {
                Blogs = new List<BlogDto>()
                {
                    new()
                    {
                        Title = createdBlog.Title,
                        Description = createdBlog.Description,
                        Category = createdBlog.Category,
                        AppUserId = createdBlog.AppUserId
                    }
                },
                Result = true,
                Message = new List<string>()
                {
                    "Blog was successfully added"
                }
            };
        throw new DAL.Exceptions.NotImplementedException("Something went wrong. Was unable to add blog post.");
    }

    public async Task<BlogDto> GetBlogByIdAsync(Guid id)
    {
        var blog = await _blogRepository.GetByIdAsync(id);

        if (blog == null)
            throw new NotFoundException("Invalid Id");

        return new BlogDto()
        {
            AppUserId = blog.AppUserId,
            Title = blog.Title,
            Description = blog.Description,
            Category = blog.Category,
            ImageUrl = blog.ImageUrl
        };
    }

    public async Task<bool> UpdateBlogAsync(UpdateBlogDto blogDto)
    {
        var user = await _userManager.FindByIdAsync(blogDto.AppUserId);

        if (user == null)
            throw new NotFoundException($"User with Id: {blogDto.AppUserId} was not found.");

        var userBlog = await _blogRepository.GetSingleByAsync(x => x.Id == blogDto.Id);

        if (userBlog != null)
        {
            userBlog.Title = blogDto.Title;
            userBlog.Description = blogDto.Description;
            userBlog.Category = blogDto.Category;

            var updatedBlog = await _blogRepository.UpdateAsync(userBlog);

            if (updatedBlog != null) return true;

            throw new DAL.Exceptions.NotImplementedException("Was unable to update your blog");
        }

        throw new DAL.Exceptions.NotImplementedException($"Blog with Id: {blogDto.Id} was not found");
    }

    public async Task<BlogResult> DeleteBlogAsync(Guid id)
    {
        var blog = await _blogRepository.GetSingleByAsync(x => x.Id == id);

        if (blog == null)
            throw new NotFoundException($"Invalid product id: {id}");

        await _blogRepository.DeleteAsync(blog);

        return new BlogResult()
        {
            Result = true,
            Message = new List<string>()
            {
                "Blog was deleted successfully"
            }
        };
    }

    public async Task<Status> UploadBlogImagesAsync(string id, IFormFile file)
    {
        var status = new Status();
        var blog = await _blogRepository.GetSingleByAsync(x => x.Id.ToString() == id);

        if (blog is null)
        {
            status.StatusCode = 0;
            status.Message = $"Invalid blog id: {id}";

            return status;
        }

        string path;
        if (file.Length > 0)
        {
            path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "Uploaded files"));
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            await using var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create);
            await file.CopyToAsync(fileStream);

            blog.ImageUrl = path + $"/{file.FileName}";

            var updatedBlog = await _blogRepository.UpdateAsync(blog);

            if (updatedBlog is null)
            {
                status.StatusCode = 0;
                status.Message = "Unable to upload image";

                return status;
            }

            status.StatusCode = 1;
            status.Message = $"Image uploaded successfully. Path: {path}";

            return status;
        }

        status.StatusCode = 0;
        status.Message = "No file found";

        return status;
    }
}
