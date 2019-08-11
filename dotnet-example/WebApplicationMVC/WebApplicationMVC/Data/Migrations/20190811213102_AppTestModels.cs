using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplicationMVC.Data.Migrations
{
    public partial class AppTestModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppTestModel",
                columns: table => new
                {
                    AppTestInput = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTestModel", x => x.AppTestInput);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppTestModel");
        }
    }
}
