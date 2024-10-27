using Microsoft.AspNetCore.Mvc;
using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.Core.Domain.Administration;
using Pcf.Administration.WebHost.Models;

namespace Pcf.Administration.WebHost.Controllers
{
	/// <summary>
	/// Employee's Roles
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class RolesController : ControllerBase
	{
		private readonly IRepository<Role> _rolesRepository;

		public RolesController(IRepository<Role> rolesRepository)
		{
			_rolesRepository = rolesRepository;
		}

		/// <summary>
		/// Get All EmployeeRoles
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IEnumerable<RoleItemResponse>> GetRolesAsync()
		{
			var roles = await _rolesRepository.GetAllAsync();

			var rolesModelList = roles.Select(x =>
				new RoleItemResponse()
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description
				}).ToList();

			return rolesModelList;
		}
	}
}
