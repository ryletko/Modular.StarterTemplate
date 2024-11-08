using Example.ReadModel.Model;
using Microsoft.EntityFrameworkCore;

namespace Example.ReadModel.Context;

internal partial class ReadDbContext : DbContext
{
    public ReadDbContext()
    {
    }

    public ReadDbContext(DbContextOptions<ReadDbContext> options)
        : base(options)
    {
    }

    public virtual IQueryable<Projects_PackagingArtwork> Projects_PackagingArtworks => Set<Projects_PackagingArtwork>().AsNoTracking();

    public virtual IQueryable<Projects_Project> Projects_Projects => Set<Projects_Project>().AsNoTracking();

    public virtual IQueryable<Projects_Status> Projects_Statuses => Set<Projects_Status>().AsNoTracking();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=modular.example;User Id=sa;Password=mssql;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Projects_PackagingArtwork>(entity =>
        {
            entity.ToTable("PackagingArtworks", "projects");

            entity.HasIndex(e => e.ProjectId, "IX_PackagingArtworks_ProjectId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Project).WithMany(p => p.PackagingArtworks).HasForeignKey(d => d.ProjectId);
        });

        modelBuilder.Entity<Projects_Project>(entity =>
        {
            entity.ToTable("Projects", "projects");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Discriminator).HasMaxLength(21);
            entity.Property(e => e.Status).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Projects_Status>(entity =>
        {
            entity.ToTable("Statuses", "projects");

            entity.HasIndex(e => e.ProjectId, "IX_Statuses_ProjectId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.StatusCode).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Project).WithMany(p => p.Statuses).HasForeignKey(d => d.ProjectId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
