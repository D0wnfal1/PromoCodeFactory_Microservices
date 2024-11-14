using System;
using System.Collections.Generic;
using System.Text;

namespace Pcf.ReceivingFromPartner.Core.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string error)
            : base(error)
        {
            
        }
    }
}
