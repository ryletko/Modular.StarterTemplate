using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Modular.Framework.Infrastructure.DataAccess.Migrations;

public static class MigrationExtension
{
    public static SqlServerDbContextOptionsBuilder UseMigrationTable(this SqlServerDbContextOptionsBuilder x, string schema)
    {
        return x.MigrationsHistoryTable("__migrations", schema);
    }
}