using Huntr.SecondFactorAuth.BL.Models;
using Huntr.SecondFactorAuth.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Huntr.SecondFactorAuth.Repository.Services
{
    public class DynamoDbDatabaseService : IDatabaseService
    {
        public Task<bool> SaveCodeAsync(SaveCodeRequest saveCodeRequest)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Contracts.ConfirmationCode>> GetCodesAsync(GetCodeRequest getCodeRequest)
        {
            throw new NotImplementedException();
        }
    }
}
