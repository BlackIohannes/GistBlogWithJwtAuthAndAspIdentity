using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GistBlog.DAL.Migrations
{
    public partial class removenameobjectfromcommententity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Comments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
