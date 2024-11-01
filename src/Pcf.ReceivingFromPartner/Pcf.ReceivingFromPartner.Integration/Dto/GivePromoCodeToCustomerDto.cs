using MongoDB.Bson;
using System;

namespace Pcf.ReceivingFromPartner.Integration.Dto
{
    public class GivePromoCodeToCustomerDto
    {
        public string ServiceInfo { get; set; }

        public ObjectId PartnerId { get; set; }

        public ObjectId PromoCodeId { get; set; }
        
        public string PromoCode { get; set; }

        public ObjectId PreferenceId { get; set; }

        public string BeginDate { get; set; }

        public string EndDate { get; set; }
        
        public ObjectId? PartnerManagerId { get; set; }
    }
}