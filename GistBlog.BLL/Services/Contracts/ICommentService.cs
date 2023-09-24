using GistBlog.DAL.Entities.DTOs;

namespace GistBlog.BLL.Services.Contracts;

public interface ICommentService
{
    Task<Status> AddCommentAsync(CommentDto comment);
}
