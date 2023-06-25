using System;

namespace Huntr.SecondFactorAuth.Contracts.Exceptions
{
    public class TooManyCodesException : Exception
    {
        public TooManyCodesException() : base()
        {

        }

        public TooManyCodesException(string message) : base(message)
        {

        }
    }
}
