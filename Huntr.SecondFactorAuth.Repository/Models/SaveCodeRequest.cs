using System;

namespace Huntr.SecondFactorAuth.BL.Models
{
    public class SaveCodeRequest
    {
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
