using Huntr.SecondFactorAuth.BL.Interfaces;
using Huntr.SecondFactorAuth.Contracts.Exceptions;
using Huntr.SecondFactorAuth.Contracts.Interfaces;
using Huntr.SecondFactorAuth.Contracts.Request;
using Huntr.SecondFactorAuth.Contracts.Response;
using Huntr.SecondFactorAuth.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Huntr.SecondFactorAuth.BL.Services
{
    public class CodeService : ICodeService
    {
        private readonly IDatabaseServiceFactory _databaseServiceFactory;
        private readonly IConfiguration _configuration;
        private readonly ICodeGenerationService _codeGenerationService;

        public CodeService(IDatabaseServiceFactory databaseServiceResolver, IConfiguration configuration, ICodeGenerationService codeGenerationService)
        {
            _databaseServiceFactory = databaseServiceResolver;
            _configuration = configuration;
            _codeGenerationService = codeGenerationService;
        }

        public async Task<SendCodeResponse> SendCodeAsync(SendCodeRequest request)
        {
            var dbType = _configuration.GetValue<string>("DatabaseType");
            var codeLifetime = _configuration.GetValue<int>("Code:LifetimeInMinutes");
            var numberOfConcurrentCodePerPhone = _configuration.GetValue<int>("Code:NumberOfConcurrentCodePerPhone");

            var dbService = _databaseServiceFactory.GetDatabaseService(dbType);
            if (dbService == null) 
            {
                throw new Exception("Invalid Database Type Configuration");
            }
            var codesForPhone = await dbService.GetCodesAsync(new Models.GetCodeRequest { PhoneNumber = $"{request.CountryCode}{request.PhoneNumber}", UtcNow = DateTime.UtcNow });
            if(codesForPhone != null && codesForPhone.Count() >= numberOfConcurrentCodePerPhone) 
            {
                throw new TooManyCodesException("Too many active codes for this phone");
            }
            var code = _codeGenerationService.GetCode();
            var response = await dbService.SaveCodeAsync(new Models.SaveCodeRequest
            {
                Code = code,
                ExpiryTime = DateTime.UtcNow.AddMinutes(codeLifetime),
                PhoneNumber = $"{request.CountryCode}{request.PhoneNumber}"
            });
            return new SendCodeResponse { IsSuccess = response };
        }

        public async Task<VerifyCodeResponse> VerifyCodeAsync(VerifyCodeRequest request)
        {
            var dbType = _configuration.GetValue<string>("DatabaseType");
            var dbService = _databaseServiceFactory.GetDatabaseService(dbType);
            if (dbService == null)
            {
                throw new Exception("Invalid Database Type Configuration");
            }
            var codesForPhone = await dbService.GetCodesAsync(new Models.GetCodeRequest { PhoneNumber = $"{request.CountryCode}{request.PhoneNumber}", UtcNow = DateTime.UtcNow });
            return new VerifyCodeResponse() { IsSuccess = codesForPhone != null && codesForPhone.Any(x => x.Code == request.ConfirmationCode) };
        }
    }
}
