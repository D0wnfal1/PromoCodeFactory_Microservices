using MongoDB.Bson;

namespace Pcf.ReceivingFromPartner.Core.Domain
{
    public class BaseEntity
    {
        public ObjectId Id { get; set; }
    }
}