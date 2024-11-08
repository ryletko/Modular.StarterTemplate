using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Modular.Framework.Infrastructure.DataAccess;
using Modular.Framework.Infrastructure.DataAccess.Migrations;
using Modular.Framework.Module.Config;

namespace Modular.Framework.Module.DataAccess;

internal record DataAccessModuleParameters(string DatabaseConnectionString,
                                           ILoggerFactory LoggerFactory,
                                           ModuleContext ModuleContext);

internal class DataAccessModule<T>(DataAccessModuleParameters parameters) : Autofac.Module where T : BaseDbContext
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TransactionAccessor>()
               .As<ITransactionAccessor>()
               .InstancePerLifetimeScope();

        builder.Register(c => new DbConnectionAccessor(parameters.DatabaseConnectionString))
               .As<IDbConnectionAccessor>()
               .InstancePerLifetimeScope();

        builder.Register(c =>
                         {
                             var dbConnectionAccessor = c.Resolve<IDbConnectionAccessor>();
                             var dbConnection = dbConnectionAccessor.Current ?? dbConnectionAccessor.Open();

                             var dbContextOptionsBuilder = new DbContextOptionsBuilder<T>();
                             dbContextOptionsBuilder.UseSqlServer(dbConnection,
                                                                  x => x.UseMigrationTable(parameters.ModuleContext.SchemaName)
                                                                        .ExecutionStrategy((e) => new NonRetryingExecutionStrategy(e.CurrentContext.Context)));

                             var dbContext = (T) Activator.CreateInstance(typeof(T), dbContextOptionsBuilder.Options, parameters.LoggerFactory);

                             return dbContext;
                         })
               .AsSelf()
               .As<DbContext>()
               .As<T>()
               .As<BaseDbContext>()
               .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(parameters.ModuleContext.InfrastructureAssembly)
               .Where(type => type.Name.EndsWith("Repository"))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope()
               .FindConstructorsWith(new AllConstructorFinder());

        // builder.RegisterType<QueryContext>()
        //        .As<IQueryContext>()
        //        .InstancePerLifetimeScope();
    }
}