
using MongoDB.Bson;

namespace Pcf.ReceivingFromPartner.Core.Domain
{
    public class PromoCode
        : BaseEntity
    {
        public string Code { get; set; }

        public string ServiceInfo { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public ObjectId PartnerId { get; set; }

        public virtual Partner Partner { get; set; }
        
        public ObjectId? PartnerManagerId { get; set; }
        
        public virtual Preference Preference { get; set; }

        public ObjectId PreferenceId { get; set; }
    }
}