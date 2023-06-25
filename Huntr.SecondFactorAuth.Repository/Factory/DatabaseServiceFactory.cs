using Huntr.SecondFactorAuth.Repository.Interfaces;
using Huntr.SecondFactorAuth.Repository.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Huntr.SecondFactorAuth.Repository.Factory
{
    public class DatabaseServiceFactory : IDatabaseServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }

        public IDatabaseService GetDatabaseService(string dbType)
        {
            IDatabaseService databaseService = null;
            switch (dbType)
            {
                case KeyStore.DynamoDb:
                    databaseService = _serviceProvider.GetService<DynamoDbDatabaseService>();
                    break;
                case KeyStore.InMemory:
                    databaseService = _serviceProvider.GetService<InMemoryDatabaseService>();
                    break;
            }
            return databaseService;
        }
    }
}
