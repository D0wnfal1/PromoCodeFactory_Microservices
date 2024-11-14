using Pcf.ReceivingFromPartner.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pcf.ReceivingFromPartner.Core.Models
{
    public class PartnerPromocode
    {
        public PromoCode Promocode { get; set; }
        public Partner Partner { get; set; }
    }
}
