using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplicationMVC.Data.Migrations
{
    public partial class AddStoreUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StoreUserId",
                table: "UserSpecificItemModel",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "UserSpecificItemModel",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSpecificItemModel_StoreUserId",
                table: "UserSpecificItemModel",
                column: "StoreUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSpecificItemModel_AspNetUsers_StoreUserId",
                table: "UserSpecificItemModel",
                column: "StoreUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSpecificItemModel_AspNetUsers_StoreUserId",
                table: "UserSpecificItemModel");

            migrationBuilder.DropIndex(
                name: "IX_UserSpecificItemModel_StoreUserId",
                table: "UserSpecificItemModel");

            migrationBuilder.DropColumn(
                name: "StoreUserId",
                table: "UserSpecificItemModel");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "UserSpecificItemModel");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}
