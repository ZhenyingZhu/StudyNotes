using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemOrganizer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAnalysisDetectionReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "analysis_detections",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    analysis_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    category = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    confidence = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    suggested_container_id = table.Column<Guid>(type: "uuid", nullable: true),
                    review_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    reviewed_name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    reviewed_description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    reviewed_category = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    reviewed_quantity = table.Column<int>(type: "integer", nullable: true),
                    selected_container_id = table.Column<Guid>(type: "uuid", nullable: true),
                    resulting_item_id = table.Column<Guid>(type: "uuid", nullable: true),
                    reviewed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    concurrency_token = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_object_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_analysis_detections", x => x.id);
                    table.UniqueConstraint("ak_analysis_detections_id_tenant_id_owner_object_id", x => new { x.id, x.tenant_id, x.owner_object_id });
                    table.CheckConstraint("ck_analysis_detections_confidence", "confidence BETWEEN 0 AND 1");
                    table.CheckConstraint("ck_analysis_detections_owner", "tenant_id <> '00000000-0000-0000-0000-000000000000' AND owner_object_id <> '00000000-0000-0000-0000-000000000000'");
                    table.CheckConstraint("ck_analysis_detections_quantity", "quantity > 0");
                    table.CheckConstraint("ck_analysis_detections_review", "(review_status = 'Pending' AND reviewed_at IS NULL AND resulting_item_id IS NULL) OR (review_status = 'Rejected' AND reviewed_at IS NOT NULL AND resulting_item_id IS NULL) OR (review_status = 'Accepted' AND reviewed_at IS NOT NULL AND resulting_item_id IS NOT NULL)");
                    table.CheckConstraint("ck_analysis_detections_timestamps", "updated_at >= created_at");
                    table.ForeignKey(
                        name: "fk_analysis_detections_analyses_analysis_id_tenant_id_owner_ob",
                        columns: x => new { x.analysis_id, x.tenant_id, x.owner_object_id },
                        principalTable: "analyses",
                        principalColumns: new[] { "id", "tenant_id", "owner_object_id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_analysis_detections_containers_suggested_container_id_tenan",
                        columns: x => new { x.suggested_container_id, x.tenant_id, x.owner_object_id },
                        principalTable: "containers",
                        principalColumns: new[] { "id", "tenant_id", "owner_object_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_analysis_detections_analysis_id_review_status",
                table: "analysis_detections",
                columns: new[] { "analysis_id", "review_status" });

            migrationBuilder.CreateIndex(
                name: "ix_analysis_detections_analysis_id_tenant_id_owner_object_id",
                table: "analysis_detections",
                columns: new[] { "analysis_id", "tenant_id", "owner_object_id" });

            migrationBuilder.CreateIndex(
                name: "ix_analysis_detections_suggested_container_id_tenant_id_owner_",
                table: "analysis_detections",
                columns: new[] { "suggested_container_id", "tenant_id", "owner_object_id" });

            migrationBuilder.CreateIndex(
                name: "ix_analysis_detections_tenant_id_owner_object_id",
                table: "analysis_detections",
                columns: new[] { "tenant_id", "owner_object_id" });

            migrationBuilder.Sql(
                """
                INSERT INTO analysis_detections (
                    id,
                    analysis_id,
                    name,
                    description,
                    category,
                    quantity,
                    confidence,
                    suggested_container_id,
                    review_status,
                    created_at,
                    updated_at,
                    concurrency_token,
                    tenant_id,
                    owner_object_id)
                SELECT
                    i.id,
                    i.analysis_id,
                    i.name,
                    i.description,
                    i.category,
                    i.quantity,
                    i.confidence,
                    a.suggested_container_id,
                    'Pending',
                    i.created_at,
                    i.updated_at,
                    i.concurrency_token,
                    i.tenant_id,
                    i.owner_object_id
                FROM items i
                JOIN item_assignments a ON a.item_id = i.id
                WHERE i.analysis_id IS NOT NULL;

                DELETE FROM item_assignments
                WHERE item_id IN (
                    SELECT id FROM items WHERE analysis_id IS NOT NULL
                );

                DELETE FROM items WHERE analysis_id IS NOT NULL;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "analysis_detections");
        }
    }
}
