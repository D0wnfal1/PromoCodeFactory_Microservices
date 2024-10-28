
namespace Pcf.ReceivingFromPartner.Core.Domain
{
    public class PromoCode
        : BaseEntity
    {
        public string Code { get; set; }

        public string ServiceInfo { get; set; }

		private DateTime _beginDate;
		public DateTime BeginDate
		{
			get => _beginDate;
			set => _beginDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
		}

		private DateTime _endDate;
		public DateTime EndDate
		{
			get => _endDate;
			set => _endDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
		}

		public Guid PartnerId { get; set; }

        public virtual Partner Partner { get; set; }
        
        public Guid? PartnerManagerId { get; set; }
        
        public virtual Preference Preference { get; set; }

        public Guid PreferenceId { get; set; }
    }
}