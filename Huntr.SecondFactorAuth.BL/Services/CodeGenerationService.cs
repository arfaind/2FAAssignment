using Huntr.SecondFactorAuth.BL.Interfaces;

namespace Huntr.SecondFactorAuth.BL.Services
{
    internal class CodeGenerationService : ICodeGenerationService
    {
        public string GetCode()
        {
            return "123";
        }
    }
}
