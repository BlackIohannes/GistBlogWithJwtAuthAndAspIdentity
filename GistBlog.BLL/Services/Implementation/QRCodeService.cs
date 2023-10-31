using GistBlog.DAL.Configurations;

namespace GistBlog.BLL.Services.Implementation;

public class QRCodeService
{
    private readonly DataContext _context;

    public QRCodeService(DataContext context)
    {
        _context = context;
    }
}
