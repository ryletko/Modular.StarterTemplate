using Example.Projects.Domain.Projects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modular.Framework.Infrastructure.DataAccess;

namespace Example.Projects.Infrastructure.DataAccess;

public class ProjectsDbContext: BaseDbContext
{
    public DbSet<Project> Projects { get; set; }
    
    public ProjectsDbContext()
    {
        
    }
    
    public ProjectsDbContext(DbContextOptions<ProjectsDbContext> options, ILoggerFactory loggerFactory)
        : base(options, loggerFactory)
    {
    }

    
    protected override string SchemaName => Schema.Name;
    protected override string MigrationConnectionStringName => "DefaultConnection";
    protected override string MigrationAppsettingsJson => "appsettings.json";
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
        base.OnConfiguring(optionsBuilder);
    }
}