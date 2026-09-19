using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemOrganizer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialPersistence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "containers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    normalized_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    location = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    labels = table.Column<string[]>(type: "text[]", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    concurrency_token = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_object_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_containers", x => x.id);
                    table.UniqueConstraint("ak_containers_id_tenant_id_owner_object_id", x => new { x.id, x.tenant_id, x.owner_object_id });
                    table.CheckConstraint("ck_containers_deleted_at", "deleted_at IS NULL OR deleted_at >= created_at");
                    table.CheckConstraint("ck_containers_owner", "tenant_id <> '00000000-0000-0000-0000-000000000000' AND owner_object_id <> '00000000-0000-0000-0000-000000000000'");
                    table.CheckConstraint("ck_containers_timestamps", "updated_at >= created_at");
                });

            migrationBuilder.CreateTable(
                name: "idempotency_records",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    operation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    request_hash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    resource_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    resource_id = table.Column<Guid>(type: "uuid", nullable: true),
                    error_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    concurrency_token = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_object_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_idempotency_records", x => x.id);
                    table.UniqueConstraint("ak_idempotency_records_id_tenant_id_owner_object_id", x => new { x.id, x.tenant_id, x.owner_object_id });
                    table.CheckConstraint("ck_idempotency_expiration", "expires_at > created_at");
                    table.CheckConstraint("ck_idempotency_records_owner", "tenant_id <> '00000000-0000-0000-0000-000000000000' AND owner_object_id <> '00000000-0000-0000-0000-000000000000'");
                    table.CheckConstraint("ck_idempotency_records_timestamps", "updated_at >= created_at");
                    table.CheckConstraint("ck_idempotency_result", "(status = 'InProgress' AND resource_id IS NULL AND error_code IS NULL) OR (status = 'Completed' AND resource_id IS NOT NULL AND resource_type IS NOT NULL AND error_code IS NULL) OR (status = 'Failed' AND resource_id IS NULL AND error_code IS NOT NULL)");
                });

            migrationBuilder.CreateTable(
                name: "photos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    blob_name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    content_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    content_length = table.Column<long>(type: "bigint", nullable: false),
                    width = table.Column<int>(type: "integer", nullable: false),
                    height = table.Column<int>(type: "integer", nullable: false),
                    sha256 = table.Column<string>(type: "character(64)", fixedLength: true, maxLength: 64, nullable: false),
                    retain_until = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    retention_state = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    deletion_requested_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    concurrency_token = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_object_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_photos", x => x.id);
                    table.UniqueConstraint("ak_photos_id_tenant_id_owner_object_id", x => new { x.id, x.tenant_id, x.owner_object_id });
                    table.CheckConstraint("ck_photos_content_length", "content_length > 0 AND content_length <= 10485760");
                    table.CheckConstraint("ck_photos_dimensions", "width BETWEEN 512 AND 8000 AND height BETWEEN 512 AND 8000");
                    table.CheckConstraint("ck_photos_owner", "tenant_id <> '00000000-0000-0000-0000-000000000000' AND owner_object_id <> '00000000-0000-0000-0000-000000000000'");
                    table.CheckConstraint("ck_photos_retention", "retain_until >= created_at");
                    table.CheckConstraint("ck_photos_sha256", "sha256 ~ '^[0-9a-f]{64}$'");
                    table.CheckConstraint("ck_photos_timestamps", "updated_at >= created_at");
                });

            migrationBuilder.CreateTable(
                name: "analyses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    photo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    delivery_attempt_count = table.Column<int>(type: "integer", nullable: false),
                    prompt_version = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    schema_version = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    application_version = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    confirmed_container_id = table.Column<Guid>(type: "uuid", nullable: true),
                    error_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    error_message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    correlation_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    warnings = table.Column<string[]>(type: "text[]", nullable: false),
                    started_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    completed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    cancellation_requested_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    cancelled_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    concurrency_token = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_object_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_analyses", x => x.id);
                    table.UniqueConstraint("ak_analyses_id_tenant_id_owner_object_id", x => new { x.id, x.tenant_id, x.owner_object_id });
                    table.CheckConstraint("ck_analyses_attempt_count", "delivery_attempt_count BETWEEN 0 AND 6");
                    table.CheckConstraint("ck_analyses_owner", "tenant_id <> '00000000-0000-0000-0000-000000000000' AND owner_object_id <> '00000000-0000-0000-0000-000000000000'");
                    table.CheckConstraint("ck_analyses_terminal_timestamp", "(status IN ('Completed', 'Failed') AND completed_at IS NOT NULL) OR (status = 'Cancelled' AND cancelled_at IS NOT NULL) OR (status IN ('Queued', 'Running') AND completed_at IS NULL AND cancelled_at IS NULL)");
                    table.CheckConstraint("ck_analyses_timestamps", "updated_at >= created_at");
                    table.ForeignKey(
                        name: "fk_analyses_containers_confirmed_container_id_tenant_id_owner_",
                        columns: x => new { x.confirmed_container_id, x.tenant_id, x.owner_object_id },
                        principalTable: "containers",
                        principalColumns: new[] { "id", "tenant_id", "owner_object_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_analyses_photos_photo_id_tenant_id_owner_object_id",
                        columns: x => new { x.photo_id, x.tenant_id, x.owner_object_id },
                        principalTable: "photos",
                        principalColumns: new[] { "id", "tenant_id", "owner_object_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    photo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    analysis_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    normalized_name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    category = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    normalized_category = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    confidence = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    deduplication_key = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    concurrency_token = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_object_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_items", x => x.id);
                    table.UniqueConstraint("ak_items_id_tenant_id_owner_object_id", x => new { x.id, x.tenant_id, x.owner_object_id });
                    table.CheckConstraint("ck_items_confidence", "confidence BETWEEN 0 AND 1");
                    table.CheckConstraint("ck_items_deleted_at", "deleted_at IS NULL OR deleted_at >= created_at");
                    table.CheckConstraint("ck_items_owner", "tenant_id <> '00000000-0000-0000-0000-000000000000' AND owner_object_id <> '00000000-0000-0000-0000-000000000000'");
                    table.CheckConstraint("ck_items_quantity", "quantity > 0");
                    table.CheckConstraint("ck_items_timestamps", "updated_at >= created_at");
                    table.ForeignKey(
                        name: "fk_items_analyses_analysis_id_tenant_id_owner_object_id",
                        columns: x => new { x.analysis_id, x.tenant_id, x.owner_object_id },
                        principalTable: "analyses",
                        principalColumns: new[] { "id", "tenant_id", "owner_object_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_items_photos_photo_id_tenant_id_owner_object_id",
                        columns: x => new { x.photo_id, x.tenant_id, x.owner_object_id },
                        principalTable: "photos",
                        principalColumns: new[] { "id", "tenant_id", "owner_object_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "outbox_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    analysis_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    attempt_count = table.Column<int>(type: "integer", nullable: false),
                    available_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    dispatched_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    last_error_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    concurrency_token = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_object_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_messages", x => x.id);
                    table.UniqueConstraint("ak_outbox_messages_id_tenant_id_owner_object_id", x => new { x.id, x.tenant_id, x.owner_object_id });
                    table.CheckConstraint("ck_outbox_attempt_count", "attempt_count >= 0");
                    table.CheckConstraint("ck_outbox_messages_owner", "tenant_id <> '00000000-0000-0000-0000-000000000000' AND owner_object_id <> '00000000-0000-0000-0000-000000000000'");
                    table.CheckConstraint("ck_outbox_messages_timestamps", "updated_at >= created_at");
                    table.ForeignKey(
                        name: "fk_outbox_messages_analyses_analysis_id_tenant_id_owner_object",
                        columns: x => new { x.analysis_id, x.tenant_id, x.owner_object_id },
                        principalTable: "analyses",
                        principalColumns: new[] { "id", "tenant_id", "owner_object_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_assignments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    source = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    container_id = table.Column<Guid>(type: "uuid", nullable: true),
                    suggested_container_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    concurrency_token = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_object_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_item_assignments", x => x.id);
                    table.UniqueConstraint("ak_item_assignments_id_tenant_id_owner_object_id", x => new { x.id, x.tenant_id, x.owner_object_id });
                    table.CheckConstraint("ck_item_assignments_owner", "tenant_id <> '00000000-0000-0000-0000-000000000000' AND owner_object_id <> '00000000-0000-0000-0000-000000000000'");
                    table.CheckConstraint("ck_item_assignments_state", "(status = 'Unassigned' AND source = 'None' AND container_id IS NULL AND suggested_container_id IS NULL) OR (status = 'Suggested' AND source = 'AiSuggestion' AND container_id IS NULL AND suggested_container_id IS NOT NULL) OR (status = 'Confirmed' AND source IN ('User', 'ConvenienceWorkflow') AND container_id IS NOT NULL AND suggested_container_id IS NULL)");
                    table.CheckConstraint("ck_item_assignments_timestamps", "updated_at >= created_at");
                    table.ForeignKey(
                        name: "fk_item_assignments_containers_container_id_tenant_id_owner_ob",
                        columns: x => new { x.container_id, x.tenant_id, x.owner_object_id },
                        principalTable: "containers",
                        principalColumns: new[] { "id", "tenant_id", "owner_object_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_item_assignments_containers_suggested_container_id_tenant_i",
                        columns: x => new { x.suggested_container_id, x.tenant_id, x.owner_object_id },
                        principalTable: "containers",
                        principalColumns: new[] { "id", "tenant_id", "owner_object_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_item_assignments_items_item_id_tenant_id_owner_object_id",
                        columns: x => new { x.item_id, x.tenant_id, x.owner_object_id },
                        principalTable: "items",
                        principalColumns: new[] { "id", "tenant_id", "owner_object_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_analyses_confirmed_container_id_tenant_id_owner_object_id",
                table: "analyses",
                columns: new[] { "confirmed_container_id", "tenant_id", "owner_object_id" });

            migrationBuilder.CreateIndex(
                name: "ix_analyses_photo_id_status",
                table: "analyses",
                columns: new[] { "photo_id", "status" });

            migrationBuilder.CreateIndex(
                name: "ix_analyses_photo_id_tenant_id_owner_object_id",
                table: "analyses",
                columns: new[] { "photo_id", "tenant_id", "owner_object_id" });

            migrationBuilder.CreateIndex(
                name: "ix_analyses_tenant_id_owner_object_id",
                table: "analyses",
                columns: new[] { "tenant_id", "owner_object_id" });

            migrationBuilder.CreateIndex(
                name: "ix_containers_tenant_id_owner_object_id",
                table: "containers",
                columns: new[] { "tenant_id", "owner_object_id" });

            migrationBuilder.CreateIndex(
                name: "ix_containers_tenant_id_owner_object_id_normalized_name",
                table: "containers",
                columns: new[] { "tenant_id", "owner_object_id", "normalized_name" },
                unique: true,
                filter: "deleted_at IS NULL");

            migrationBuilder.CreateIndex(
                name: "ix_idempotency_records_expires_at",
                table: "idempotency_records",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "ix_idempotency_records_tenant_id_owner_object_id",
                table: "idempotency_records",
                columns: new[] { "tenant_id", "owner_object_id" });

            migrationBuilder.CreateIndex(
                name: "ix_idempotency_records_tenant_id_owner_object_id_operation_key",
                table: "idempotency_records",
                columns: new[] { "tenant_id", "owner_object_id", "operation", "key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_item_assignments_container_id_tenant_id_owner_object_id",
                table: "item_assignments",
                columns: new[] { "container_id", "tenant_id", "owner_object_id" });

            migrationBuilder.CreateIndex(
                name: "ix_item_assignments_item_id",
                table: "item_assignments",
                column: "item_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_item_assignments_item_id_tenant_id_owner_object_id",
                table: "item_assignments",
                columns: new[] { "item_id", "tenant_id", "owner_object_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_item_assignments_suggested_container_id_tenant_id_owner_obj",
                table: "item_assignments",
                columns: new[] { "suggested_container_id", "tenant_id", "owner_object_id" });

            migrationBuilder.CreateIndex(
                name: "ix_item_assignments_tenant_id_owner_object_id",
                table: "item_assignments",
                columns: new[] { "tenant_id", "owner_object_id" });

            migrationBuilder.CreateIndex(
                name: "ix_items_analysis_id_deduplication_key",
                table: "items",
                columns: new[] { "analysis_id", "deduplication_key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_items_analysis_id_tenant_id_owner_object_id",
                table: "items",
                columns: new[] { "analysis_id", "tenant_id", "owner_object_id" });

            migrationBuilder.CreateIndex(
                name: "ix_items_photo_id_tenant_id_owner_object_id",
                table: "items",
                columns: new[] { "photo_id", "tenant_id", "owner_object_id" });

            migrationBuilder.CreateIndex(
                name: "ix_items_tenant_id_owner_object_id",
                table: "items",
                columns: new[] { "tenant_id", "owner_object_id" });

            migrationBuilder.CreateIndex(
                name: "ix_items_tenant_id_owner_object_id_normalized_name",
                table: "items",
                columns: new[] { "tenant_id", "owner_object_id", "normalized_name" });

            migrationBuilder.CreateIndex(
                name: "ix_outbox_messages_analysis_id",
                table: "outbox_messages",
                column: "analysis_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_outbox_messages_analysis_id_tenant_id_owner_object_id",
                table: "outbox_messages",
                columns: new[] { "analysis_id", "tenant_id", "owner_object_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_outbox_messages_status_available_at",
                table: "outbox_messages",
                columns: new[] { "status", "available_at" });

            migrationBuilder.CreateIndex(
                name: "ix_outbox_messages_tenant_id_owner_object_id",
                table: "outbox_messages",
                columns: new[] { "tenant_id", "owner_object_id" });

            migrationBuilder.CreateIndex(
                name: "ix_photos_retention_state_retain_until",
                table: "photos",
                columns: new[] { "retention_state", "retain_until" });

            migrationBuilder.CreateIndex(
                name: "ix_photos_tenant_id_owner_object_id",
                table: "photos",
                columns: new[] { "tenant_id", "owner_object_id" });

            migrationBuilder.CreateIndex(
                name: "ix_photos_tenant_id_owner_object_id_sha256",
                table: "photos",
                columns: new[] { "tenant_id", "owner_object_id", "sha256" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "idempotency_records");

            migrationBuilder.DropTable(
                name: "item_assignments");

            migrationBuilder.DropTable(
                name: "outbox_messages");

            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropTable(
                name: "analyses");

            migrationBuilder.DropTable(
                name: "containers");

            migrationBuilder.DropTable(
                name: "photos");
        }
    }
}
