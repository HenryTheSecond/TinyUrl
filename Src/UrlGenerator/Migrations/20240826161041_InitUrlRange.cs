using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlGenerator.Migrations
{
    public partial class InitUrlRange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using var streamReader = new StreamReader("SqlScripts/InitUrlRange.txt");
            string insertStatement;
            while((insertStatement = streamReader.ReadLine()) != null)
            {
                migrationBuilder.Sql(insertStatement);
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
