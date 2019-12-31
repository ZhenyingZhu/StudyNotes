using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplicationMVC.Data.Migrations
{
    public partial class ApplyIdentityChange : Migration
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
                name: "UserSpecificItemModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    UserID = table.Column<int>(nullable: false),
                    StoreUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSpecificItemModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSpecificItemModel_AspNetUsers_StoreUserId",
                        column: x => x.StoreUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppTestChildModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ParentID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTestChildModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppTestChildModels_AppTestModel_ParentID",
                        column: x => x.ParentID,
                        principalTable: "AppTestModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppTestModel",
                columns: new[] { "Id", "AppTestInput" },
                values: new object[] { 10, "Seeding Test1" });

            migrationBuilder.CreateIndex(
                name: "IX_AppTestChildModels_ParentID",
                table: "AppTestChildModels",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSpecificItemModel_StoreUserId",
                table: "UserSpecificItemModel",
                column: "StoreUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppTestChildModels");

            migrationBuilder.DropTable(
                name: "UserSpecificItemModel");

            migrationBuilder.DropTable(
                name: "AppTestModel");
        }
    }
}
