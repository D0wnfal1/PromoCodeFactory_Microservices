using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.WebHost.Mappers;
using Pcf.GivingToCustomer.WebHost.Models;

namespace Pcf.GivingToCustomer.WebHost.Controllers
{
	/// <summary>
	/// Promocodes
	/// </summary>
	[ApiController]
	[Route("api/v1/[controller]")]
	public class PromocodesController : ControllerBase
	{
		private readonly IRepository<PromoCode> _promoCodesRepository;
		private readonly IRepository<Preference> _preferencesRepository;
		private readonly IRepository<Customer> _customersRepository;

		public PromocodesController(IRepository<PromoCode> promoCodesRepository,
			IRepository<Preference> preferencesRepository, IRepository<Customer> customersRepository)
		{
			_promoCodesRepository = promoCodesRepository;
			_preferencesRepository = preferencesRepository;
			_customersRepository = customersRepository;
		}

		/// <summary>
		/// Get All Promocodes
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
		{
			var promocodes = await _promoCodesRepository.GetAllAsync();

			var response = promocodes.Select(x => new PromoCodeShortResponse()
			{
				Id = x.Id,
				Code = x.Code,
				BeginDate = x.BeginDate.ToString("yyyy-MM-dd"),
				EndDate = x.EndDate.ToString("yyyy-MM-dd"),
				PartnerId = x.PartnerId,
				ServiceInfo = x.ServiceInfo
			}).ToList();

			return Ok(response);
		}

		/// <summary>
		/// Create promocode with Preference
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
		{
			//Get Preference by Name
			var preference = await _preferencesRepository.GetByIdAsync(request.PreferenceId);

			if (preference == null)
			{
				return BadRequest();
			}

			//  Get Customers with this Preference
			var customers = await _customersRepository
				.GetWhere(d => d.Preferences.Any(x =>
					x.Preference.Id == preference.Id));

			PromoCode promoCode = PromoCodeMapper.MapFromModel(request, preference, customers);

			await _promoCodesRepository.AddAsync(promoCode);

			return CreatedAtAction(nameof(GetPromocodesAsync), new { }, null);
		}
	}
}
