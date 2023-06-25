using System;

namespace Huntr.SecondFactorAuth.Contracts.Request
{
    public class VerifyCodeRequest : SendCodeRequest
    {
        public string ConfirmationCode { get; set; }
        public DateTime UtcNow { get; set; }
    }
}
