using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplicationMVC.Data.Migrations
{
    public partial class AppTestChildModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppTestChildModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    AppTestModelAppTestInput = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTestChildModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppTestChildModels_AppTestModel_AppTestModelAppTestInput",
                        column: x => x.AppTestModelAppTestInput,
                        principalTable: "AppTestModel",
                        principalColumn: "AppTestInput",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppTestChildModels_AppTestModelAppTestInput",
                table: "AppTestChildModels",
                column: "AppTestModelAppTestInput");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppTestChildModels");
        }
    }
}
