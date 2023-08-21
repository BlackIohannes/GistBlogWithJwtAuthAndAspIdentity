using GistBlog.DAL.Entities.Models;
using GistBlog.DAL.Entities.Models.Domain;
using GistBlog.DAL.Entities.Tokens;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GistBlog.DAL.Configurations;

public class DataContext : IdentityDbContext<AppUser>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Blog>? Blogs { get; set; }
    public DbSet<TokenInfo>? TokenInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Blog>().HasQueryFilter(b => !b.IsDeleted);
    }
}
