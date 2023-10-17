using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Entities.DTOs;
using GistBlog.DAL.Entities.Models;
using GistBlog.DAL.Repository.Contracts;

namespace GistBlog.BLL.Services.Implementation;

public class CommentService : ICommentService
{
    private readonly IRepository<Comment> _commentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CommentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _commentRepository = _unitOfWork.GetRepository<Comment>();
    }

    public async Task<Status> AddCommentAsync(CommentDto commentDto)
    {
        var status = new Status();

        var comment = new Comment
        {
            Description = commentDto.Description,
            BlogId = commentDto.BlogId,
            AppUserId = commentDto.AppUserId
        };

        if (commentDto.AppUserId == null)
        {
            status.Message = "User not found";
            status.StatusCode = 404;
            return status;
        }

        await _commentRepository.AddAsync(comment);
        await _unitOfWork.SaveChangesAsync();

        status.Message = "Comment added successfully";
        status.StatusCode = 201;
        return status;
    }
}
