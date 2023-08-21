using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GistBlog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BlogController : ControllerBase
{
    private readonly IBlogService _blogService;

    public BlogController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    [SwaggerOperation(Summary = "AddBlog")]
    [HttpPost("CreateNewBlogPost")]
    public async Task<IActionResult> AddBlog([FromBody] BlogDto blogDto)
    {
        var result = await _blogService.AddBlogAsync(blogDto);

        if (result == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return CreatedAtAction(nameof(AddBlog), result);
    }

    [SwaggerOperation(Summary = "GetAllBlogsAsync")]
    [HttpGet("GetAllBlogsAsync")]
    public async Task<IActionResult> GetAllBlogsAsync(int pageIndex, int pageSize)
    {
        var blogs = await _blogService.GetAllBlogsAsync(pageIndex, pageSize);

        if (blogs == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(blogs);
    }

    [SwaggerOperation(Summary = "GetBlogs")]
    [HttpGet("GetAllUserBlogs")]
    public async Task<IActionResult> GetAllUserBlogsAsync(string id, int pageIndex, int pageSize)
    {
        var blogs = await _blogService.GetAllUserBlogsAsync(id, pageIndex, pageSize);

        if (blogs == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(blogs);
    }

    [SwaggerOperation(Summary = "GetBlogById")]
    [HttpGet("GetSingleBlogById")]
    public async Task<IActionResult> GetBlogById(Guid id)
    {
        var blog = await _blogService.GetBlogByIdAsync(id);

        if (blog == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(blog);
    }

    [SwaggerOperation(Summary = "UpdateBlog")]
    [HttpPost("UpdateUserBlog")]
    public async Task<IActionResult> UpdateBlog([FromBody] UpdateBlogDto blogDto)
    {
        var blog = await _blogService.UpdateBlogAsync(blogDto);
        return Ok(blog);
    }

    [SwaggerOperation(Summary = "DeleteBlog")]
    // [Authorize(Roles = "Admin")]
    [HttpDelete("DeleteBlogById")]
    public async Task<IActionResult> DeleteBlog(Guid id)
    {
        var blog = await _blogService.DeleteBlogAsync(id);

        if (blog == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(blog);
    }

    [SwaggerOperation(Summary = "UploadBlogImages")]
    [HttpPost("UploadBlogImages")]
    public async Task<IActionResult> UploadBlogImages(string id, IFormFile file)
    {
        var blog = await _blogService.UploadBlogImagesAsync(id, file);

        if (blog == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(blog);
    }

    [HttpGet("GetAllBlogsIncludingDeletedBlogs")]
    public async Task<IActionResult> GetAllBlogsIncludingDeletedBlogs()
    {
        var blogs = await _blogService.GetAllBlogsIncludingDeletedBlogs();

        if (blogs == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(blogs);
    }
}
