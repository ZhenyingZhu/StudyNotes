using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemOrganizer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewedBoundingBoxesAndItemCrops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "crop_blob_name",
                table: "items",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "crop_content_length",
                table: "items",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "crop_content_type",
                table: "items",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "crop_deletion_pending_at",
                table: "items",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "crop_height",
                table: "items",
                type: "numeric(7,6)",
                precision: 7,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "crop_pixel_height",
                table: "items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "crop_pixel_width",
                table: "items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "crop_sha256",
                table: "items",
                type: "character(64)",
                fixedLength: true,
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "crop_width",
                table: "items",
                type: "numeric(7,6)",
                precision: 7,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "crop_x",
                table: "items",
                type: "numeric(7,6)",
                precision: 7,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "crop_y",
                table: "items",
                type: "numeric(7,6)",
                precision: 7,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "predicted_bounding_box_height",
                table: "analysis_detections",
                type: "numeric(7,6)",
                precision: 7,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "predicted_bounding_box_width",
                table: "analysis_detections",
                type: "numeric(7,6)",
                precision: 7,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "predicted_bounding_box_x",
                table: "analysis_detections",
                type: "numeric(7,6)",
                precision: 7,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "predicted_bounding_box_y",
                table: "analysis_detections",
                type: "numeric(7,6)",
                precision: 7,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "reviewed_bounding_box_height",
                table: "analysis_detections",
                type: "numeric(7,6)",
                precision: 7,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "reviewed_bounding_box_width",
                table: "analysis_detections",
                type: "numeric(7,6)",
                precision: 7,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "reviewed_bounding_box_x",
                table: "analysis_detections",
                type: "numeric(7,6)",
                precision: 7,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "reviewed_bounding_box_y",
                table: "analysis_detections",
                type: "numeric(7,6)",
                precision: 7,
                scale: 6,
                nullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "ck_items_crop",
                table: "items",
                sql: "(crop_blob_name IS NULL AND crop_x IS NULL AND crop_y IS NULL AND crop_width IS NULL AND crop_height IS NULL AND crop_pixel_width IS NULL AND crop_pixel_height IS NULL AND crop_content_type IS NULL AND crop_content_length IS NULL AND crop_sha256 IS NULL AND crop_deletion_pending_at IS NULL) OR (crop_blob_name IS NOT NULL AND crop_x IS NOT NULL AND crop_y IS NOT NULL AND crop_width IS NOT NULL AND crop_height IS NOT NULL AND crop_pixel_width IS NOT NULL AND crop_pixel_height IS NOT NULL AND crop_content_type IS NOT NULL AND crop_content_length IS NOT NULL AND crop_sha256 IS NOT NULL AND crop_x >= 0 AND crop_y >= 0 AND crop_width > 0 AND crop_height > 0 AND crop_x + crop_width <= 1 AND crop_y + crop_height <= 1 AND crop_pixel_width > 0 AND crop_pixel_height > 0 AND crop_content_length > 0 AND crop_sha256 ~ '^[0-9a-f]{64}$')");

            migrationBuilder.AddCheckConstraint(
                name: "ck_analysis_detections_predicted_box",
                table: "analysis_detections",
                sql: "(predicted_bounding_box_x IS NULL AND predicted_bounding_box_y IS NULL AND predicted_bounding_box_width IS NULL AND predicted_bounding_box_height IS NULL) OR (predicted_bounding_box_x IS NOT NULL AND predicted_bounding_box_y IS NOT NULL AND predicted_bounding_box_width IS NOT NULL AND predicted_bounding_box_height IS NOT NULL AND predicted_bounding_box_x >= 0 AND predicted_bounding_box_y >= 0 AND predicted_bounding_box_width > 0 AND predicted_bounding_box_height > 0 AND predicted_bounding_box_x + predicted_bounding_box_width <= 1 AND predicted_bounding_box_y + predicted_bounding_box_height <= 1)");

            migrationBuilder.AddCheckConstraint(
                name: "ck_analysis_detections_reviewed_box",
                table: "analysis_detections",
                sql: "(reviewed_bounding_box_x IS NULL AND reviewed_bounding_box_y IS NULL AND reviewed_bounding_box_width IS NULL AND reviewed_bounding_box_height IS NULL) OR (reviewed_bounding_box_x IS NOT NULL AND reviewed_bounding_box_y IS NOT NULL AND reviewed_bounding_box_width IS NOT NULL AND reviewed_bounding_box_height IS NOT NULL AND reviewed_bounding_box_x >= 0 AND reviewed_bounding_box_y >= 0 AND reviewed_bounding_box_width > 0 AND reviewed_bounding_box_height > 0 AND reviewed_bounding_box_x + reviewed_bounding_box_width <= 1 AND reviewed_bounding_box_y + reviewed_bounding_box_height <= 1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_items_crop",
                table: "items");

            migrationBuilder.DropCheckConstraint(
                name: "ck_analysis_detections_predicted_box",
                table: "analysis_detections");

            migrationBuilder.DropCheckConstraint(
                name: "ck_analysis_detections_reviewed_box",
                table: "analysis_detections");

            migrationBuilder.DropColumn(
                name: "crop_blob_name",
                table: "items");

            migrationBuilder.DropColumn(
                name: "crop_content_length",
                table: "items");

            migrationBuilder.DropColumn(
                name: "crop_content_type",
                table: "items");

            migrationBuilder.DropColumn(
                name: "crop_deletion_pending_at",
                table: "items");

            migrationBuilder.DropColumn(
                name: "crop_height",
                table: "items");

            migrationBuilder.DropColumn(
                name: "crop_pixel_height",
                table: "items");

            migrationBuilder.DropColumn(
                name: "crop_pixel_width",
                table: "items");

            migrationBuilder.DropColumn(
                name: "crop_sha256",
                table: "items");

            migrationBuilder.DropColumn(
                name: "crop_width",
                table: "items");

            migrationBuilder.DropColumn(
                name: "crop_x",
                table: "items");

            migrationBuilder.DropColumn(
                name: "crop_y",
                table: "items");

            migrationBuilder.DropColumn(
                name: "predicted_bounding_box_height",
                table: "analysis_detections");

            migrationBuilder.DropColumn(
                name: "predicted_bounding_box_width",
                table: "analysis_detections");

            migrationBuilder.DropColumn(
                name: "predicted_bounding_box_x",
                table: "analysis_detections");

            migrationBuilder.DropColumn(
                name: "predicted_bounding_box_y",
                table: "analysis_detections");

            migrationBuilder.DropColumn(
                name: "reviewed_bounding_box_height",
                table: "analysis_detections");

            migrationBuilder.DropColumn(
                name: "reviewed_bounding_box_width",
                table: "analysis_detections");

            migrationBuilder.DropColumn(
                name: "reviewed_bounding_box_x",
                table: "analysis_detections");

            migrationBuilder.DropColumn(
                name: "reviewed_bounding_box_y",
                table: "analysis_detections");
        }
    }
}
