using GistBlog.DAL.Entities.DTOs;
using GistBlog.DAL.Entities.Models;

namespace GistBlog.DAL.Entities.Responses;

public class BlogResult
{
    public List<BlogDto> Blogs { get; set; }
    public bool Result { get; set; }
    public List<string> Message { get; set; }
}