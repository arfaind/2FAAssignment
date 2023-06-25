using Huntr.SecondFactorAuth.BL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Huntr.SecondFactorAuth.Repository.Interfaces
{
    public interface IDatabaseService
    {
        public Task<bool> SaveCodeAsync(SaveCodeRequest saveCodeRequest);
        public Task<IEnumerable<Contracts.ConfirmationCode>> GetCodesAsync(GetCodeRequest getCodeRequest);
    }
}
