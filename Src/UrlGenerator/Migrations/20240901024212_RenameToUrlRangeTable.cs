using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlGenerator.Migrations
{
    public partial class RenameToUrlRangeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UrlRanges",
                table: "UrlRanges");

            migrationBuilder.RenameTable(
                name: "UrlRanges",
                newName: "UrlRange");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UrlRange",
                table: "UrlRange",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UrlRange",
                table: "UrlRange");

            migrationBuilder.RenameTable(
                name: "UrlRange",
                newName: "UrlRanges");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UrlRanges",
                table: "UrlRanges",
                column: "Id");
        }
    }
}
