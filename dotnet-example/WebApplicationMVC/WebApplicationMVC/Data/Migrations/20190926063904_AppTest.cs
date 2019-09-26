using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplicationMVC.Data.Migrations
{
    public partial class AppTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppTestModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppTestInput = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTestModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppTestChildModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    AppTestModelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTestChildModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppTestChildModels_AppTestModel_AppTestModelId",
                        column: x => x.AppTestModelId,
                        principalTable: "AppTestModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AppTestModel",
                columns: new[] { "Id", "AppTestInput" },
                values: new object[] { 10, "Seeding Test1" });

            migrationBuilder.CreateIndex(
                name: "IX_AppTestChildModels_AppTestModelId",
                table: "AppTestChildModels",
                column: "AppTestModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppTestChildModels");

            migrationBuilder.DropTable(
                name: "AppTestModel");
        }
    }
}
