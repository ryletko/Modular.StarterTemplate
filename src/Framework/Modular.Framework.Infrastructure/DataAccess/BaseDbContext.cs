using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Modular.Framework.Infrastructure.DataAccess.Converters;
using Modular.Framework.Infrastructure.DataAccess.Migrations;

namespace Modular.Framework.Infrastructure.DataAccess;

public abstract class BaseDbContext : DbContext
{
    private readonly ILoggerFactory _loggerFactory;

    protected BaseDbContext()
    {
    }

    protected BaseDbContext(DbContextOptions options, ILoggerFactory loggerFactory) : base(options)
    {
        _loggerFactory = loggerFactory;
    }

    protected abstract string SchemaName { get; }
    protected abstract string MigrationConnectionStringName { get; }
    protected abstract string MigrationAppsettingsJson { get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                                              .SetBasePath(Directory.GetCurrentDirectory())
                                              .AddJsonFile(MigrationAppsettingsJson)
                                              .Build();
            var connectionString = configuration.GetConnectionString(MigrationConnectionStringName);
            optionsBuilder.UseSqlServer(connectionString, x => x.UseMigrationTable(SchemaName));
        }

        optionsBuilder.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
        optionsBuilder.EnableSensitiveDataLogging();

        base.OnConfiguring(optionsBuilder);
    }
}