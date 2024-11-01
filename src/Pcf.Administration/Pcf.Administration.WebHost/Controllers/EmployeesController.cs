using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.Core.Domain.Administration;
using Pcf.Administration.WebHost.Models;
using System.Text.Json;

namespace Pcf.Administration.WebHost.Controllers
{
	/// <summary>
	/// Employees
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeesController : ControllerBase
	{
		private readonly IRepository<Employee> _employeeRepository;
		private readonly IDistributedCache _cache;

		public EmployeesController(IRepository<Employee> employeeRepository, IDistributedCache cache)
		{
			_employeeRepository = employeeRepository;
			_cache = cache;
		}

		/// <summary>
		/// Get All Employees
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
		{
			var cacheKey = "employeesList";
			var employeesModelList = await _cache.GetStringAsync(cacheKey);

			if (string.IsNullOrEmpty(employeesModelList))
			{
				var employees = await _employeeRepository.GetAllAsync();
				employeesModelList = JsonSerializer.Serialize(employees.Select(x =>
					new EmployeeShortResponse()
					{
						Id = x.Id,
						Email = x.Email,
						FullName = x.FullName,
					}).ToList());

				var options = new DistributedCacheEntryOptions()
					.SetSlidingExpiration(TimeSpan.FromMinutes(5)); 

				await _cache.SetStringAsync(cacheKey, employeesModelList, options);
			}

			return JsonSerializer.Deserialize<List<EmployeeShortResponse>>(employeesModelList);
		}

		/// <summary>
		/// Get Employee by id
		/// </summary>
		/// <param name="id">Employee's Id <example>451533d5-d8d5-4a11-9c7b-eb9f14e1a32f</example></param>
		/// <returns></returns>
		[HttpGet("{id:guid}")]
		public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
		{
			var employee = await _employeeRepository.GetByIdAsync(id);

			if (employee == null)
				return NotFound();

			var employeeModel = new EmployeeResponse()
			{
				Id = employee.Id,
				Email = employee.Email,
				Role = new RoleItemResponse()
				{
					Id = employee.Id,
					Name = employee.Role.Name,
					Description = employee.Role.Description
				},
				FullName = employee.FullName,
				AppliedPromocodesCount = employee.AppliedPromocodesCount
			};

			return employeeModel;
		}

		/// <summary>
		/// Update Promocode Counter
		/// </summary>
		/// <param name="id">Employee's Id <example>451533d5-d8d5-4a11-9c7b-eb9f14e1a32f</example></param>
		/// <returns></returns>
		[HttpPost("{id:guid}/appliedPromocodes")]

		public async Task<IActionResult> UpdateAppliedPromocodesAsync(Guid id)
		{
			var employee = await _employeeRepository.GetByIdAsync(id);

			if (employee == null)
				return NotFound();

			employee.AppliedPromocodesCount++;

			await _employeeRepository.UpdateAsync(employee);

			return Ok();
		}
	}
}
