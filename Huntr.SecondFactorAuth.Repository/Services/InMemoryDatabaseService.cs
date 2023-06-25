using Huntr.SecondFactorAuth.BL.Models;
using Huntr.SecondFactorAuth.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Huntr.SecondFactorAuth.Repository.Services
{
    public class InMemoryDatabaseService : IDatabaseService
    {
        public Task<bool> SaveCodeAsync(SaveCodeRequest saveCodeRequest)
        {
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Contracts.ConfirmationCode>> GetCodesAsync(GetCodeRequest getCodeRequest)
        {
            if(getCodeRequest.PhoneNumber == "+1234567890")
            {
                return Task.FromResult<IEnumerable<Contracts.ConfirmationCode>>(new List<Contracts.ConfirmationCode>
                {
                    new Contracts.ConfirmationCode { Code = "123", ExpiryTime = System.DateTime.UtcNow.AddDays(10), PhoneNumber = "+1234567890"}
                });
            }
            else if (getCodeRequest.PhoneNumber == "+2234567890")
            {
                return Task.FromResult<IEnumerable<Contracts.ConfirmationCode>>(new List<Contracts.ConfirmationCode>
                {
                    new Contracts.ConfirmationCode { Code = "123", ExpiryTime = System.DateTime.UtcNow.AddDays(10), PhoneNumber = "+2234567890"},
                    new Contracts.ConfirmationCode { Code = "223", ExpiryTime = System.DateTime.UtcNow.AddDays(10), PhoneNumber = "+2234567890"}
                });
            }
            return Task.FromResult<IEnumerable<Contracts.ConfirmationCode>>(null);
        }
    }
}
