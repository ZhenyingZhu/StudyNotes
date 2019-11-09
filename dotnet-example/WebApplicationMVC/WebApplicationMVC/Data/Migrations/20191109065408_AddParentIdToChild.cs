using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplicationMVC.Data.Migrations
{
    public partial class AddParentIdToChild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTestChildModels_AppTestModel_ParentId",
                table: "AppTestChildModels");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "AppTestChildModels",
                newName: "ParentID");

            migrationBuilder.RenameIndex(
                name: "IX_AppTestChildModels_ParentId",
                table: "AppTestChildModels",
                newName: "IX_AppTestChildModels_ParentID");

            migrationBuilder.AlterColumn<int>(
                name: "ParentID",
                table: "AppTestChildModels",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppTestChildModels_AppTestModel_ParentID",
                table: "AppTestChildModels",
                column: "ParentID",
                principalTable: "AppTestModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTestChildModels_AppTestModel_ParentID",
                table: "AppTestChildModels");

            migrationBuilder.RenameColumn(
                name: "ParentID",
                table: "AppTestChildModels",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_AppTestChildModels_ParentID",
                table: "AppTestChildModels",
                newName: "IX_AppTestChildModels_ParentId");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "AppTestChildModels",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_AppTestChildModels_AppTestModel_ParentId",
                table: "AppTestChildModels",
                column: "ParentId",
                principalTable: "AppTestModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
