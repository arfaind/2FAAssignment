namespace Huntr.SecondFactorAuth.Contracts.Request
{
    public class SendCodeRequest
    {
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
