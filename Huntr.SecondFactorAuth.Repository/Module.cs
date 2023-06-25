using Huntr.SecondFactorAuth.Repository.Factory;
using Huntr.SecondFactorAuth.Repository.Interfaces;
using Huntr.SecondFactorAuth.Repository.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Huntr.SecondFactorAuth.Repository
{
    public class Module
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            services.AddScoped<DynamoDbDatabaseService>();
            services.AddScoped<InMemoryDatabaseService>();
            services.AddScoped<IDatabaseServiceFactory, DatabaseServiceFactory>();
        }
    }
}
