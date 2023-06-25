using Huntr.SecondFactorAuth.Contracts.Request;
using Huntr.SecondFactorAuth.Contracts.Response;
using System.Threading.Tasks;

namespace Huntr.SecondFactorAuth.Contracts.Interfaces
{
    public interface ICodeService
    {
        Task<SendCodeResponse> SendCodeAsync (SendCodeRequest request);
        Task<VerifyCodeResponse> VerifyCodeAsync (VerifyCodeRequest request);
    }
}