namespace GistBlog.DAL.Entities.DTOs;

public class VerifyPaymentRequest
{
    public string UserId { get; set; }
    public string ReferenceCode { get; set; }
}