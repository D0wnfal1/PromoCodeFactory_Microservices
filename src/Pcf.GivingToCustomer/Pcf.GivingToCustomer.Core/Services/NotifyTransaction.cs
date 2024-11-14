using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Core.Services
{
    public class NotifyTransaction
    {
        public string CardNumber { get; set; } = null!;
        public decimal Amount { get; set; }
    }
}
