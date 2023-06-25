using System.Collections.Generic;

namespace Huntr.SecondFactorAuth.Contracts.Response
{
    public class ErrorInfo
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ErrorInfo> Infos { get; set; }
    }
}
