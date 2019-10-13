using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplicationMVC.Data.Migrations
{
    public partial class AddParentRefToAppTestChildModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTestChildModels_AppTestModel_AppTestModelId",
                table: "AppTestChildModels");

            migrationBuilder.RenameColumn(
                name: "AppTestModelId",
                table: "AppTestChildModels",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_AppTestChildModels_AppTestModelId",
                table: "AppTestChildModels",
                newName: "IX_AppTestChildModels_ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppTestChildModels_AppTestModel_ParentId",
                table: "AppTestChildModels",
                column: "ParentId",
                principalTable: "AppTestModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTestChildModels_AppTestModel_ParentId",
                table: "AppTestChildModels");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "AppTestChildModels",
                newName: "AppTestModelId");

            migrationBuilder.RenameIndex(
                name: "IX_AppTestChildModels_ParentId",
                table: "AppTestChildModels",
                newName: "IX_AppTestChildModels_AppTestModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppTestChildModels_AppTestModel_AppTestModelId",
                table: "AppTestChildModels",
                column: "AppTestModelId",
                principalTable: "AppTestModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
