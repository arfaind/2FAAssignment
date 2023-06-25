using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huntr.SecondFactorAuth.Contracts
{
    public class ConfirmationCode
    {
        public string PhoneNumber { get; set; }
        public DateTime ExpiryTime { get; set; }
        public string Code { get; set; }
    }
}
