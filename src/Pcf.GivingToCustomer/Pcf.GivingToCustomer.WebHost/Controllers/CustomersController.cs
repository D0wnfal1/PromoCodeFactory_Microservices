using Microsoft.AspNetCore.Mvc;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.WebHost.Mappers;
using Pcf.GivingToCustomer.WebHost.Models;

namespace Pcf.GivingToCustomer.WebHost.Controllers
{
	/// <summary>
	/// Customers
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class CustomersController : ControllerBase
	{
		private readonly IRepository<Customer> _customerRepository;
		private readonly IRepository<Preference> _preferenceRepository;
        public CustomersController(IRepository<Customer> customerRepository, IRepository<Preference> preferenceRepository)
        {
            _customerRepository = customerRepository;
			_preferenceRepository = preferenceRepository;
        }

		/// <summary>
		/// Get All Customers
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult<List<CustomerShortResponse>>> GetCustomersAsync()
		{
			var customers = await _customerRepository.GetAllAsync();
			var response = customers.Select(x => new CustomerShortResponse 
			{
				Id = x.Id,
				FirstName = x.FirstName,
				LastName = x.LastName,
				Email = x.Email,
			});

			return Ok(response);
		}

		/// <summary>
		/// Get Customer by Id
		/// </summary>
		/// <param name="id">Customer Id, <example>a6c8c6b1-4349-45b0-ab31-244740aaf0f0</example></param>
		/// <returns></returns>
		[HttpGet("{id:guid}")]
		public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
		{
			var customer = await _customerRepository.GetByIdAsync(id);

			var response = new CustomerResponse(customer);
			return Ok(response);
		}

		/// <summary>
		/// Create new Customer
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task<ActionResult<CustomerResponse>> CreateCustomerAsync(CreateOrEditCustomerRequest request)
		{
			var preferences = await _preferenceRepository.GetRangeByIdsAsync(request.PreferenceIds);

			Customer customer = CustomerMapper.MapFromModel(request, preferences);

			await _customerRepository.AddAsync(customer);
			return CreatedAtAction(nameof(GetCustomerAsync), new { id = customer.Id }, customer.Id);
		}

		/// <summary>
		/// Update Customer
		/// </summary>
		/// <param name="id">Customer Id, <example>a6c8c6b1-4349-45b0-ab31-244740aaf0f0</example></param>
		/// <param name="request">Request Data></param>
		[HttpPut("{id:guid}")]
		public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
		{
			var customer = await _customerRepository.GetByIdAsync(id);

			if (customer == null)
			{
				return NotFound();
			}

			await _customerRepository.UpdateAsync(customer);
			return Ok();
		}

		/// <summary>
		/// Delete Customer
		/// </summary>
		/// <param name="id">Customer Id, <example>a6c8c6b1-4349-45b0-ab31-244740aaf0f0</example></param>
		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> DeleteCustomerAsync(Guid id)
		{
			var customer = await _customerRepository.GetByIdAsync(id);
			if(customer == null)
			{
				return NotFound();
			}

			await _customerRepository.DeleteAsync(customer);
			return Ok();
		}
	}
}
