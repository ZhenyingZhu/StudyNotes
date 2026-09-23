using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemOrganizer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddManualItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_items_confidence",
                table: "items");

            migrationBuilder.AlterColumn<Guid>(
                name: "photo_id",
                table: "items",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<decimal>(
                name: "confidence",
                table: "items",
                type: "numeric(5,4)",
                precision: 5,
                scale: 4,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,4)",
                oldPrecision: 5,
                oldScale: 4);

            migrationBuilder.AlterColumn<Guid>(
                name: "analysis_id",
                table: "items",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddCheckConstraint(
                name: "ck_items_confidence",
                table: "items",
                sql: "confidence IS NULL OR confidence BETWEEN 0 AND 1");

            migrationBuilder.AddCheckConstraint(
                name: "ck_items_source",
                table: "items",
                sql: "(photo_id IS NULL AND analysis_id IS NULL AND confidence IS NULL) OR (photo_id IS NOT NULL AND analysis_id IS NOT NULL AND confidence IS NOT NULL)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_items_confidence",
                table: "items");

            migrationBuilder.DropCheckConstraint(
                name: "ck_items_source",
                table: "items");

            migrationBuilder.AlterColumn<Guid>(
                name: "photo_id",
                table: "items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "confidence",
                table: "items",
                type: "numeric(5,4)",
                precision: 5,
                scale: 4,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,4)",
                oldPrecision: 5,
                oldScale: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "analysis_id",
                table: "items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "ck_items_confidence",
                table: "items",
                sql: "confidence BETWEEN 0 AND 1");
        }
    }
}
