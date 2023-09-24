using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GistBlog.API.Controllers;

[ApiController]
[Route("api/v1/")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost("comments")]
    public async Task<IActionResult> CreateComment(CommentDto commentDto)
    {
        var result = await _commentService.AddCommentAsync(commentDto);
        return CreatedAtAction(nameof(CreateComment), result);
    }
}
