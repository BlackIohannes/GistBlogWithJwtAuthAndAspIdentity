using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GistBlog.DAL.Migrations
{
    public partial class emailimplementation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VerificationToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationToken",
                table: "AspNetUsers");
        }
    }
}
