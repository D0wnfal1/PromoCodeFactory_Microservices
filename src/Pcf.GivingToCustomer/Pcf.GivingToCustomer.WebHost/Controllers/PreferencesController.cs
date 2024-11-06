using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.WebHost.Models;

namespace Pcf.GivingToCustomer.WebHost.Controllers
{
	/// <summary>
	/// Предпочтения клиентов
	/// </summary>
	[ApiController]
	[Route("api/v1/[controller]")]
	public class PreferencesController : ControllerBase
	{
		private readonly IRepository<Preference> _preferencesRepository;

		public PreferencesController(IRepository<Preference> preferencesRepository)
		{
			_preferencesRepository = preferencesRepository;
		}

		/// <summary>
		/// Get All Preferences
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult<List<PreferenceResponse>>> GetPreferencesAsync()
		{
			var prefences = await _preferencesRepository.GetAllAsync();
			var preferencesResponse = prefences.Select(x => new PreferenceResponse()
			{
				Id = x.Id,
				Name = x.Name,
			});

			return Ok(preferencesResponse);
		}

		/// <summary>
		/// Get Preference by Id
		/// </summary>
		/// <returns></returns>
		[HttpGet("{id}")]
		public async Task<ActionResult<PreferenceResponse>> GetPreferencesByIdAsync(Guid id)
		{
			var preference = await _preferencesRepository.GetByIdAsync(id);

			if (preference == null)
			{
				return NotFound();
			}

			var response = new PreferenceResponse() { Id = id, Name = preference.Name };

			return Ok(response);
		}
	}
}
