using ItemOrganizer.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItemOrganizer.Infrastructure;

public sealed class ItemOrganizerDbContext(DbContextOptions<ItemOrganizerDbContext> options)
    : DbContext(options)
{
    public DbSet<StorageContainer> Containers => Set<StorageContainer>();

    public DbSet<Photo> Photos => Set<Photo>();

    public DbSet<Analysis> Analyses => Set<Analysis>();

    public DbSet<Item> Items => Set<Item>();

    public DbSet<ItemAssignment> ItemAssignments => Set<ItemAssignment>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public DbSet<IdempotencyRecord> IdempotencyRecords => Set<IdempotencyRecord>();

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        RefreshConcurrencyTokens();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        RefreshConcurrencyTokens();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureContainer(modelBuilder.Entity<StorageContainer>());
        ConfigurePhoto(modelBuilder.Entity<Photo>());
        ConfigureAnalysis(modelBuilder.Entity<Analysis>());
        ConfigureItem(modelBuilder.Entity<Item>());
        ConfigureAssignment(modelBuilder.Entity<ItemAssignment>());
        ConfigureOutbox(modelBuilder.Entity<OutboxMessage>());
        ConfigureIdempotency(modelBuilder.Entity<IdempotencyRecord>());
    }

    private void RefreshConcurrencyTokens()
    {
        foreach (var entry in ChangeTracker.Entries<Entity>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Property(nameof(Entity.ConcurrencyToken)).CurrentValue = Guid.NewGuid();
            }
        }
    }

    private static void ConfigureContainer(EntityTypeBuilder<StorageContainer> builder)
    {
        ConfigureOwnedEntity(builder, "containers");
        builder.Property(entity => entity.Name).HasMaxLength(200).IsRequired();
        builder.Property(entity => entity.NormalizedName).HasMaxLength(200).IsRequired();
        builder.Property(entity => entity.Description).HasMaxLength(2_000);
        builder.Property(entity => entity.Location).HasMaxLength(500);
        builder.Property(entity => entity.Labels).HasColumnType("text[]").IsRequired();
        builder.HasIndex(entity => new
        {
            entity.TenantId,
            entity.OwnerObjectId,
            entity.NormalizedName
        }).IsUnique().HasFilter("deleted_at IS NULL");
        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_containers_deleted_at",
                "deleted_at IS NULL OR deleted_at >= created_at");
        });
    }

    private static void ConfigurePhoto(EntityTypeBuilder<Photo> builder)
    {
        ConfigureOwnedEntity(builder, "photos");
        builder.Property(entity => entity.BlobName).HasMaxLength(1_024).IsRequired();
        builder.Property(entity => entity.ContentType).HasMaxLength(100).IsRequired();
        builder.Property(entity => entity.Sha256).HasMaxLength(64).IsFixedLength().IsRequired();
        builder.Property(entity => entity.RetentionState).HasConversion<string>().HasMaxLength(50);
        builder.HasIndex(entity => new
        {
            entity.TenantId,
            entity.OwnerObjectId,
            entity.Sha256
        });
        builder.HasIndex(entity => new { entity.RetentionState, entity.RetainUntil });
        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_photos_content_length",
                "content_length > 0 AND content_length <= 10485760");
            table.HasCheckConstraint(
                "ck_photos_dimensions",
                "width BETWEEN 512 AND 8000 AND height BETWEEN 512 AND 8000");
            table.HasCheckConstraint(
                "ck_photos_sha256",
                "sha256 ~ '^[0-9a-f]{64}$'");
            table.HasCheckConstraint(
                "ck_photos_retention",
                "retain_until >= created_at");
        });
    }

    private static void ConfigureAnalysis(EntityTypeBuilder<Analysis> builder)
    {
        ConfigureOwnedEntity(builder, "analyses");
        builder.Property(entity => entity.Status).HasConversion<string>().HasMaxLength(50);
        builder.Property(entity => entity.PromptVersion).HasMaxLength(100).IsRequired();
        builder.Property(entity => entity.SchemaVersion).HasMaxLength(100).IsRequired();
        builder.Property(entity => entity.Model).HasMaxLength(100).IsRequired();
        builder.Property(entity => entity.ApplicationVersion).HasMaxLength(100).IsRequired();
        builder.Property(entity => entity.ErrorCode).HasMaxLength(100);
        builder.Property(entity => entity.ErrorMessage).HasMaxLength(500);
        builder.Property(entity => entity.CorrelationId).HasMaxLength(100);
        builder.Property(entity => entity.Warnings).HasColumnType("text[]").IsRequired();
        builder.HasOne<Photo>()
            .WithMany()
            .HasForeignKey(entity => new
            {
                entity.PhotoId,
                entity.TenantId,
                entity.OwnerObjectId
            })
            .HasPrincipalKey(entity => new
            {
                entity.Id,
                entity.TenantId,
                entity.OwnerObjectId
            })
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<StorageContainer>()
            .WithMany()
            .HasForeignKey(entity => new
            {
                entity.ConfirmedContainerId,
                entity.TenantId,
                entity.OwnerObjectId
            })
            .HasPrincipalKey(entity => new
            {
                entity.Id,
                entity.TenantId,
                entity.OwnerObjectId
            })
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(entity => new { entity.PhotoId, entity.Status });
        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_analyses_attempt_count",
                "delivery_attempt_count BETWEEN 0 AND 6");
            table.HasCheckConstraint(
                "ck_analyses_terminal_timestamp",
                "(status IN ('Completed', 'Failed') AND completed_at IS NOT NULL) OR " +
                "(status = 'Cancelled' AND cancelled_at IS NOT NULL) OR " +
                "(status IN ('Queued', 'Running') AND completed_at IS NULL AND cancelled_at IS NULL)");
        });
    }

    private static void ConfigureItem(EntityTypeBuilder<Item> builder)
    {
        ConfigureOwnedEntity(builder, "items");
        builder.Property(entity => entity.Name).HasMaxLength(300).IsRequired();
        builder.Property(entity => entity.NormalizedName).HasMaxLength(300).IsRequired();
        builder.Property(entity => entity.Description).HasMaxLength(2_000);
        builder.Property(entity => entity.Category).HasMaxLength(200);
        builder.Property(entity => entity.NormalizedCategory).HasMaxLength(200);
        builder.Property(entity => entity.Confidence).HasPrecision(5, 4);
        builder.Property(entity => entity.DeduplicationKey).HasMaxLength(600).IsRequired();
        builder.HasOne<Photo>()
            .WithMany()
            .HasForeignKey(entity => new
            {
                entity.PhotoId,
                entity.TenantId,
                entity.OwnerObjectId
            })
            .HasPrincipalKey(entity => new
            {
                entity.Id,
                entity.TenantId,
                entity.OwnerObjectId
            })
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Analysis>()
            .WithMany()
            .HasForeignKey(entity => new
            {
                entity.AnalysisId,
                entity.TenantId,
                entity.OwnerObjectId
            })
            .HasPrincipalKey(entity => new
            {
                entity.Id,
                entity.TenantId,
                entity.OwnerObjectId
            })
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(entity => new { entity.AnalysisId, entity.DeduplicationKey }).IsUnique();
        builder.HasIndex(entity => new
        {
            entity.TenantId,
            entity.OwnerObjectId,
            entity.NormalizedName
        });
        builder.ToTable(table =>
        {
            table.HasCheckConstraint("ck_items_quantity", "quantity > 0");
            table.HasCheckConstraint("ck_items_confidence", "confidence BETWEEN 0 AND 1");
            table.HasCheckConstraint(
                "ck_items_deleted_at",
                "deleted_at IS NULL OR deleted_at >= created_at");
        });
    }

    private static void ConfigureAssignment(EntityTypeBuilder<ItemAssignment> builder)
    {
        ConfigureOwnedEntity(builder, "item_assignments");
        builder.Property(entity => entity.Status).HasConversion<string>().HasMaxLength(50);
        builder.Property(entity => entity.Source).HasConversion<string>().HasMaxLength(50);
        builder.HasIndex(entity => entity.ItemId).IsUnique();
        builder.HasOne<Item>()
            .WithOne(entity => entity.Assignment)
            .HasForeignKey<ItemAssignment>(entity => new
            {
                entity.ItemId,
                entity.TenantId,
                entity.OwnerObjectId
            })
            .HasPrincipalKey<Item>(entity => new
            {
                entity.Id,
                entity.TenantId,
                entity.OwnerObjectId
            })
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<StorageContainer>()
            .WithMany()
            .HasForeignKey(entity => new
            {
                entity.ContainerId,
                entity.TenantId,
                entity.OwnerObjectId
            })
            .HasPrincipalKey(entity => new
            {
                entity.Id,
                entity.TenantId,
                entity.OwnerObjectId
            })
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<StorageContainer>()
            .WithMany()
            .HasForeignKey(entity => new
            {
                entity.SuggestedContainerId,
                entity.TenantId,
                entity.OwnerObjectId
            })
            .HasPrincipalKey(entity => new
            {
                entity.Id,
                entity.TenantId,
                entity.OwnerObjectId
            })
            .OnDelete(DeleteBehavior.Restrict);
        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_item_assignments_state",
                "(status = 'Unassigned' AND source = 'None' AND container_id IS NULL AND suggested_container_id IS NULL) OR " +
                "(status = 'Suggested' AND source = 'AiSuggestion' AND container_id IS NULL AND suggested_container_id IS NOT NULL) OR " +
                "(status = 'Confirmed' AND source IN ('User', 'ConvenienceWorkflow') AND container_id IS NOT NULL AND suggested_container_id IS NULL)");
        });
    }

    private static void ConfigureOutbox(EntityTypeBuilder<OutboxMessage> builder)
    {
        ConfigureOwnedEntity(builder, "outbox_messages");
        builder.Property(entity => entity.Status).HasConversion<string>().HasMaxLength(50);
        builder.Property(entity => entity.LastErrorCode).HasMaxLength(100);
        builder.HasIndex(entity => entity.AnalysisId).IsUnique();
        builder.HasIndex(entity => new { entity.Status, entity.AvailableAt });
        builder.HasOne<Analysis>()
            .WithOne()
            .HasForeignKey<OutboxMessage>(entity => new
            {
                entity.AnalysisId,
                entity.TenantId,
                entity.OwnerObjectId
            })
            .HasPrincipalKey<Analysis>(entity => new
            {
                entity.Id,
                entity.TenantId,
                entity.OwnerObjectId
            })
            .OnDelete(DeleteBehavior.Cascade);
        builder.ToTable(table =>
        {
            table.HasCheckConstraint("ck_outbox_attempt_count", "attempt_count >= 0");
        });
    }

    private static void ConfigureIdempotency(EntityTypeBuilder<IdempotencyRecord> builder)
    {
        ConfigureOwnedEntity(builder, "idempotency_records");
        builder.Property(entity => entity.Operation).HasMaxLength(100).IsRequired();
        builder.Property(entity => entity.Key).HasMaxLength(200).IsRequired();
        builder.Property(entity => entity.RequestHash).HasMaxLength(128).IsRequired();
        builder.Property(entity => entity.Status).HasConversion<string>().HasMaxLength(50);
        builder.Property(entity => entity.ResourceType).HasMaxLength(100);
        builder.Property(entity => entity.ErrorCode).HasMaxLength(100);
        builder.HasIndex(entity => new
        {
            entity.TenantId,
            entity.OwnerObjectId,
            entity.Operation,
            entity.Key
        }).IsUnique();
        builder.HasIndex(entity => entity.ExpiresAt);
        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "ck_idempotency_expiration",
                "expires_at > created_at");
            table.HasCheckConstraint(
                "ck_idempotency_result",
                "(status = 'InProgress' AND resource_id IS NULL AND error_code IS NULL) OR " +
                "(status = 'Completed' AND resource_id IS NOT NULL AND resource_type IS NOT NULL AND error_code IS NULL) OR " +
                "(status = 'Failed' AND resource_id IS NULL AND error_code IS NOT NULL)");
        });
    }

    private static void ConfigureOwnedEntity<TEntity>(
        EntityTypeBuilder<TEntity> builder,
        string tableName)
        where TEntity : OwnedEntity
    {
        builder.ToTable(tableName, table =>
        {
            table.HasCheckConstraint(
                $"ck_{tableName}_owner",
                "tenant_id <> '00000000-0000-0000-0000-000000000000' AND " +
                "owner_object_id <> '00000000-0000-0000-0000-000000000000'");
            table.HasCheckConstraint(
                $"ck_{tableName}_timestamps",
                "updated_at >= created_at");
        });
        builder.HasKey(entity => entity.Id);
        builder.HasAlternateKey(entity => new
        {
            entity.Id,
            entity.TenantId,
            entity.OwnerObjectId
        });
        builder.Property(entity => entity.Id).ValueGeneratedNever();
        builder.Property(entity => entity.ConcurrencyToken).IsConcurrencyToken();
        builder.HasIndex(entity => new { entity.TenantId, entity.OwnerObjectId });
    }
}
