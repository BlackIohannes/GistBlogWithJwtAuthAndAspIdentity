using System.Diagnostics;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Configurations;
using GistBlog.DAL.Entities.DTOs;
using GistBlog.DAL.Entities.Models;
using GistBlog.DAL.Entities.Models.Domain;
using GistBlog.DAL.Entities.Responses;
using GistBlog.DAL.Exceptions;
using GistBlog.DAL.Repository.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KeyNotFoundException = GistBlog.DAL.Exceptions.KeyNotFoundException;
using NotImplementedException = GistBlog.DAL.Exceptions.NotImplementedException;

namespace GistBlog.BLL.Services.Implementation;

public class BlogService : IBlogService
{
    private readonly string _apiKey = "797915466192946";
    private readonly IRepository<Blog> _blogRepository;
    private readonly string _cloudName = "dmz8tpotk";
    private readonly DataContext _context;
    private readonly string _secretKey = "g5iSlFlb7ebmv45I3lCQRjamVEU";
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;

    public BlogService(DataContext context, IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _blogRepository = _unitOfWork.GetRepository<Blog>();
        _userManager = userManager;
    }

    public async Task<PaginatedListService<BlogDto>> GetAllBlogsAsync(int pageIndex, int pageSize)
    {
        var queryableBlogs = _blogRepository.GetQueryable().OrderByDescending(x => x.DateCreated);

        var paginatedBlogs = await PaginatedListService<BlogDto>.CreateAsync(
            queryableBlogs.Select(x => new BlogDto
            {
                AppUserId = x.AppUserId,
                Title = x.Title,
                Description = x.Description,
                Category = x.Category
                // ImageUrl = x.ImageUrl
            }),
            pageIndex,
            pageSize
        );

        return paginatedBlogs;
    }

    public async Task<BlogResult> AddBlogAsync(BlogDto blogDto)
    {
        var _cloudinary = new Cloudinary(new Account(_cloudName, _apiKey, _secretKey));
        var result = new ImageUploadResult();
        var file = blogDto.File;

        var userExist = await _userManager.FindByIdAsync(blogDto.AppUserId);

        if (userExist == null)
            throw new KeyNotFoundException(
                $"User Id: {blogDto.AppUserId} does not match with the post.");

        if (file?.Length > 0)
        {
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill")
            };

            result = await _cloudinary.UploadAsync(uploadParams);

            var newBlog = new Blog
            {
                AppUserId = blogDto.AppUserId,
                Title = blogDto.Title,
                Description = blogDto.Description,
                Category = blogDto.Category,
                ImageUrl = result.SecureUrl.ToString()
            };

            var createdBlog = await _blogRepository.AddAsync(newBlog);

            if (createdBlog != null)

                return new BlogResult
                {
                    Blogs = new List<BlogDto>
                    {
                        new()
                        {
                            Title = createdBlog.Title,
                            Description = createdBlog.Description,
                            Category = createdBlog.Category,
                            AppUserId = createdBlog.AppUserId
                            // ImageUrl = createdBlog.ImageUrl
                        }
                    },
                    Result = true,
                    Message = new List<string>
                    {
                        "Blog was successfully added"
                    }
                };
        }

        throw new NotImplementedException("Something went wrong. Was unable to add blog post.");
    }

    public async Task<BlogDto> GetBlogByIdAsync(Guid id)
    {
        var blog = await _blogRepository.GetByIdAsync(id);

        if (blog == null)
            throw new NotFoundException("Invalid Id");

        return new BlogDto
        {
            AppUserId = blog.AppUserId,
            Title = blog.Title,
            Description = blog.Description,
            Category = blog.Category
            // ImageUrl = blog.ImageUrl
        };
    }

    public async Task<bool> UpdateBlogAsync(UpdateBlogDto blogDto)
    {
        var _cloudinary = new Cloudinary(new Account(_cloudName, _apiKey, _secretKey));
        var result = new ImageUploadResult();
        var file = blogDto.File;

        var user = await _userManager.FindByIdAsync(blogDto.AppUserId);

        if (user == null)
            throw new NotFoundException($"User with Id: {blogDto.AppUserId} was not found.");

        var userBlog = await _blogRepository.GetSingleByAsync(x => x.Id == blogDto.Id);

        if (userBlog != null && file?.Length > 0)
        {
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill")
            };

            result = await _cloudinary.UploadAsync(uploadParams);

            userBlog.Title = blogDto.Title;
            userBlog.Description = blogDto.Description;
            userBlog.Category = blogDto.Category;
            userBlog.ImageUrl = result.SecureUrl.ToString();

            var updatedBlog = await _blogRepository.UpdateAsync(userBlog);

            if (updatedBlog != null) return true;

            throw new NotImplementedException("Was unable to update your blog");
        }

        throw new NotImplementedException($"Blog with Id: {blogDto.Id} was not found");
    }

    public async Task<BlogResult> DeleteBlogAsync(Guid id)
    {
        var blog = await _blogRepository.GetSingleByAsync(x => x.Id == id);

        if (blog == null)
            throw new NotFoundException($"Invalid product id: {id}");

        // soft delete
        blog.IsDeleted = true;
        blog.DateDeleted = DateTime.Now;

        await _blogRepository.UpdateAsync(blog);

        return new BlogResult
        {
            Result = true,
            Message = new List<string>
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

    public async Task<IEnumerable<BlogDto>> GetAllBlogsIncludingDeletedBlogs()
    {
        Debug.Assert(_context.Blogs != null, "_context.Blogs != null");
        var blogs = await _context.Blogs.IgnoreQueryFilters().ToListAsync();

        var blogDtos = blogs.Select(blog => new BlogDto
        {
            Title = blog.Title,
            Description = blog.Description,
            Category = blog.Category,
            // ImageUrl = blog.ImageUrl,
            AppUserId = blog.AppUserId
        }).ToList();

        return blogDtos;
    }

    public async Task<PaginatedListService<BlogDto>> GetAllUserBlogsAsync(string id, int pageIndex, int pageSize)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            throw new NotFoundException("User not found.");

        var blogsQuery = _blogRepository.GetQueryable(x => x.AppUserId == id).OrderByDescending(x => x.DateCreated);

        if (blogsQuery == null)
            throw new NotFoundException("Blogs not found.");

        var paginatedBlogs = await PaginatedListService<Blog>.CreateAsync(
            blogsQuery,
            pageIndex,
            pageSize
        );

        var blogDtos = paginatedBlogs.Select(x => new BlogDto
        {
            AppUserId = x.AppUserId,
            Title = x.Title,
            Description = x.Description,
            Category = x.Category
            // ImageUrl = x.ImageUrl
        });

        return new PaginatedListService<BlogDto>(blogDtos, paginatedBlogs.TotalCount, pageIndex, pageSize);
    }

    public Task<PaginatedListService<BlogDto>> GetAllBlogsAsync(int pageIndex, int pageSize, string sortBy,
        string sortOrder,
        string searchTerm)
    {
        throw new System.NotImplementedException();
    }
}
