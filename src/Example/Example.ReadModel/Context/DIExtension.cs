using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Example.ReadModel.Context;

public static class DIExtension
{
    public static IServiceCollection AddReadDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ReadDbContext>(o => o.UseSqlServer(connectionString));
        services.AddScoped<IDataQuery>(c => new DataQuery(c.GetService<ReadDbContext>()));
        return services;
    }
    
}