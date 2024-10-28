
namespace Pcf.ReceivingFromPartner.Core.Domain
{
    public class 
        PartnerPromoCodeLimit : BaseEntity
    {
		public Guid PartnerId { get; set; }

		public virtual Partner Partner { get; set; }

		private DateTime _createDate;
		public DateTime CreateDate
		{
			get => _createDate;
			set => _createDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
		}

		private DateTime? _cancelDate;
		public DateTime? CancelDate
		{
			get => _cancelDate;
			set
			{
				if (value.HasValue)
				{
					_cancelDate = DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);
				}
				else
				{
					_cancelDate = null;
				}
			}
		}

		private DateTime _endDate;
		public DateTime EndDate
		{
			get => _endDate;
			set => _endDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
		}

		public int Limit { get; set; }
	}
}