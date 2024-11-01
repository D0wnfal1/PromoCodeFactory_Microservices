using MongoDB.Bson;
using System;

namespace Pcf.ReceivingFromPartner.WebHost.Models
{
    public class PartnerPromoCodeLimitResponse
    {
        public ObjectId Id { get; set; }

        public ObjectId PartnerId { get; set; }

        public string CreateDate { get; set; }

        public string CancelDate { get; set; }

        public string EndDate { get; set; }

        public int Limit { get; set; }
    }
}