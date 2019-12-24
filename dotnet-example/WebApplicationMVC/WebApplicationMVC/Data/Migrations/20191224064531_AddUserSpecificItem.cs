using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplicationMVC.Data.Migrations
{
    public partial class AddUserSpecificItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_UserSpecificItemModel_StoreUserId",
                table: "UserSpecificItemModel",
                column: "StoreUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSpecificItemModel");
        }
    }
}
