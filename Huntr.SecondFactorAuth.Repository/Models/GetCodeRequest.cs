using System;

namespace Huntr.SecondFactorAuth.BL.Models
{
    public class GetCodeRequest
    {
        public string PhoneNumber { get; set; }
        public DateTime UtcNow { get; set; }
    }
}
