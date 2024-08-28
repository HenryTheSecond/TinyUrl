using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlGenerator.Migrations
{
    public partial class AddUrlRangeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UrlRanges",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(26)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlRanges", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlRanges");
        }
    }
}
