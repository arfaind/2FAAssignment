namespace Huntr.SecondFactorAuth.Repository.Interfaces
{
    public interface IDatabaseServiceFactory
    {
        public IDatabaseService GetDatabaseService(string dbType);
    }
}
