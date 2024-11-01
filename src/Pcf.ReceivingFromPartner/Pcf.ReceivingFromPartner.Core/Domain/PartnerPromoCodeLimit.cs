
using MongoDB.Bson;

namespace Pcf.ReceivingFromPartner.Core.Domain
{
    public class 
        PartnerPromoCodeLimit : BaseEntity
    {

        public ObjectId PartnerId { get; set; }

        public virtual Partner Partner { get; set; }
        
        public DateTime CreateDate { get; set; }

        public DateTime? CancelDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Limit { get; set; }
    }
}