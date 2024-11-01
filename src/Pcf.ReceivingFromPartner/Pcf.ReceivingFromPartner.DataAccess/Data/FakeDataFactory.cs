using MongoDB.Bson;
using Pcf.ReceivingFromPartner.Core.Domain;

namespace Pcf.ReceivingFromPartner.DataAccess.Data
{
	public static class FakeDataFactory
	{
		public static List<Preference> Preferences => new List<Preference>()
		{
			new Preference()
			{
				Id = ObjectId.Parse("60d5ecf6a1f56c1f9c1a5a45"), // Пример корректного ObjectId
                Name = "Theater",
			},
			new Preference()
			{
				Id = ObjectId.Parse("60d5ecf6a1f56c1f9c1a5a46"), // Пример корректного ObjectId
                Name = "Family",
			},
			new Preference()
			{
				Id = ObjectId.Parse("60d5ecf6a1f56c1f9c1a5a47"), // Пример корректного ObjectId
                Name = "Kids",
			}
		};

		public static List<Partner> Partners => new List<Partner>()
		{
			new Partner()
			{
				Id = ObjectId.Parse("60d5ecf6a1f56c1f9c1a5a48"),
                Name = "Superheroes",
				IsActive = true,
				PartnerLimits = new List<PartnerPromoCodeLimit>()
				{
					new PartnerPromoCodeLimit()
					{
						Id = ObjectId.Parse("60d5ecf6a1f56c1f9c1a5a49"),
                        CreateDate = new DateTime(2020, 07, 9),
						EndDate = new DateTime(2020, 10, 9),
						Limit = 100
					}
				}
			},
			new Partner()
			{
				Id = ObjectId.Parse("60d5ecf6a1f56c1f9c1a5a4a"), 
                Name = "Cats",
				IsActive = true,
				PartnerLimits = new List<PartnerPromoCodeLimit>()
				{
					new PartnerPromoCodeLimit()
					{
						Id = ObjectId.Parse("60d5ecf6a1f56c1f9c1a5a4b"),
                        CreateDate = new DateTime(2020, 05, 3),
						EndDate = new DateTime(2020, 10, 15),
						CancelDate = new DateTime(2020, 06, 16),
						Limit = 1000
					},
					new PartnerPromoCodeLimit()
					{
						Id = ObjectId.Parse("60d5ecf6a1f56c1f9c1a5a4c"), 
                        CreateDate = new DateTime(2020, 05, 3),
						EndDate = new DateTime(2020, 10, 15),
						Limit = 100
					},
				}
			},
			new Partner()
			{
				Id = ObjectId.Parse("60d5ecf6a1f56c1f9c1a5a4d"), 
                Name = "Fish",
				IsActive = false,
				PartnerLimits = new List<PartnerPromoCodeLimit>()
				{
					new PartnerPromoCodeLimit()
					{
						Id = ObjectId.Parse("60d5ecf6a1f56c1f9c1a5a4e"),
                        CreateDate = new DateTime(2020, 07, 3),
						EndDate = DateTime.Now.AddMonths(1),
						Limit = 100
					}
				}
			},
			new Partner()
			{
				Id = ObjectId.Parse("60d5ecf6a1f56c1f9c1a5a4f"),
                Name = "PromoTheater",
				IsActive = false,
				PartnerLimits = new List<PartnerPromoCodeLimit>()
				{
					new PartnerPromoCodeLimit()
					{
						Id = ObjectId.Parse("60d5ecf6a1f56c1f9c1a5a50"),
                        CreateDate = new DateTime(2020, 09, 6),
						EndDate = DateTime.Now.AddMonths(1),
						Limit = 15
					}
				}
			}
		};
	}
}
