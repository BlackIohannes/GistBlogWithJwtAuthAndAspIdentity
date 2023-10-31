namespace GistBlog.DAL.Entities.Models;

public class QRCode : BaseModel
{
    public string Content { get; set; }
    public byte[] QRCodeImage { get; set; }
}
