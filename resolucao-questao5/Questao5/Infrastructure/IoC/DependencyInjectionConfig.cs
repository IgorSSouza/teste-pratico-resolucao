using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Repositories;
using Questao5.Infrastructure.Database.QueryStore.Repository;
using Questao5.Infrastructure.Sqlite;
using System.Data;
using System.Reflection;

namespace Questao5.Infrastructure.IoC
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            // MediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // Sqlite config
            services.AddSingleton(new DatabaseConfig
            {
                Name = configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite")
            });

            services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();
            services.AddSingleton<ICurrentAccountQueryRepository, CurrentAccountQueryRepository>();
            services.AddSingleton<IAccountMovementQueryRepository, AccountMovementQueryRepository>();
            services.AddSingleton<IIdEmpotenciaQueryRepository, IdEmpotenciaQueryRepository>();

            services.AddTransient<IDbConnection>(sp =>
            {
                var config = sp.GetRequiredService<DatabaseConfig>();
                return new SqliteConnection(config.Name);
            });

            return services;
        }
    }
}
