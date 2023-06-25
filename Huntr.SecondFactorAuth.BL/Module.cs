using Huntr.SecondFactorAuth.BL.Interfaces;
using Huntr.SecondFactorAuth.BL.Services;
using Huntr.SecondFactorAuth.Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Huntr.SecondFactorAuth.BL
{
    public class Module
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            services.AddScoped<ICodeService, CodeService>();
            services.AddScoped<ICodeGenerationService, CodeGenerationService>();
        }
    }
}
