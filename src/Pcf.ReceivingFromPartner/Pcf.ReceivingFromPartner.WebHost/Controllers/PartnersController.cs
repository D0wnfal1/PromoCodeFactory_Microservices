﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using Pcf.ReceivingFromPartner.Core.Abstractions.Repositories;
using Pcf.ReceivingFromPartner.Core.Domain;
using Pcf.ReceivingFromPartner.WebHost.Mappers;
using Pcf.ReceivingFromPartner.WebHost.Models;


namespace Pcf.ReceivingFromPartner.WebHost.Controllers
{
    /// <summary>
    /// Partners
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PartnersController
        : ControllerBase
    {
        private readonly IRepository<Partner> _partnersRepository;
        private readonly IRepository<Preference> _preferencesRepository;
        private readonly INotificationGateway _notificationGateway;
        private readonly IGivingPromoCodeToCustomerGateway _givingPromoCodeToCustomerGateway;
        private readonly IAdministrationGateway _administrationGateway;

        public PartnersController(IRepository<Partner> partnersRepository,
            IRepository<Preference> preferencesRepository, 
            INotificationGateway notificationGateway,
            IGivingPromoCodeToCustomerGateway givingPromoCodeToCustomerGateway,
            IAdministrationGateway administrationGateway)
        {
            _partnersRepository = partnersRepository;
            _preferencesRepository = preferencesRepository;
            _notificationGateway = notificationGateway;
            _givingPromoCodeToCustomerGateway = givingPromoCodeToCustomerGateway;
            _administrationGateway = administrationGateway;
        }

        /// <summary>
        /// Get All Partners
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<PartnerResponse>>> GetPartnersAsync()
        {
            var partners = await _partnersRepository.GetAllAsync();

            var response = partners.Select(x => new PartnerResponse()
            {
                Id = x.Id,
                Name = x.Name,
                NumberIssuedPromoCodes = x.NumberIssuedPromoCodes,
                IsActive = true,
                PartnerLimits = x.PartnerLimits
                    .Select(y => new PartnerPromoCodeLimitResponse()
                    {
                        Id = y.Id,
                        PartnerId = y.PartnerId,
                        Limit = y.Limit,
                        CreateDate = y.CreateDate.ToString("dd.MM.yyyy hh:mm:ss"),
                        EndDate = y.EndDate.ToString("dd.MM.yyyy hh:mm:ss"),
                        CancelDate = y.CancelDate?.ToString("dd.MM.yyyy hh:mm:ss"),
                    }).ToList()
            });

            return Ok(response);
        }
        
        /// <summary>
        /// Get All Partner's Information
        /// </summary>
        /// <param name="id">Partner Id, <example>20d2d612-db93-4ed5-86b1-ff2413bca655</example></param>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<List<PartnerResponse>>> GetPartnersAsync(ObjectId id)
        {
            var partner = await _partnersRepository.GetByIdAsync(id);

            if (partner == null)
            {
                return NotFound();
            }

            var response = new PartnerResponse()
            {
                Id = partner.Id,
                Name = partner.Name,
                NumberIssuedPromoCodes = partner.NumberIssuedPromoCodes,
                IsActive = true,
                PartnerLimits = partner.PartnerLimits
                    .Select(y => new PartnerPromoCodeLimitResponse()
                    {
                        Id = y.Id,
                        PartnerId = y.PartnerId,
                        Limit = y.Limit,
                        CreateDate = y.CreateDate.ToString("dd.MM.yyyy hh:mm:ss"),
                        EndDate = y.EndDate.ToString("dd.MM.yyyy hh:mm:ss"),
                        CancelDate = y.CancelDate?.ToString("dd.MM.yyyy hh:mm:ss"),
                    }).ToList()
            };

            return Ok(response);
        }

		/// <summary>
		/// Set Partner Promo Code Limit
		/// </summary>
		[HttpPost("{id:guid}/limits")]
        public async Task<IActionResult> SetPartnerPromoCodeLimitAsync(ObjectId id, SetPartnerPromoCodeLimitRequest request)
        {
            var partner = await _partnersRepository.GetByIdAsync(id);

            if (partner == null)
                return NotFound();
            
            if (!partner.IsActive)
                return BadRequest("Partner is unactive");
            
            var activeLimit = partner.PartnerLimits.FirstOrDefault(x => 
                !x.CancelDate.HasValue);
            
            if (activeLimit != null)
            {
                partner.NumberIssuedPromoCodes = 0;
                
                activeLimit.CancelDate = DateTime.Now;
            }

            if (request.Limit <= 0)
                return BadRequest("Limit should be grater than 0");
            
            var newLimit = new PartnerPromoCodeLimit()
            {
                Limit = request.Limit,
                Partner = partner,
                PartnerId = partner.Id,
                CreateDate = DateTime.Now,
                EndDate = request.EndDate
            };
            
            partner.PartnerLimits.Add(newLimit);

            await _partnersRepository.UpdateAsync(partner);
            
            await _notificationGateway
                .SendNotificationToPartnerAsync(partner.Id, "You have promocodes limit");
            
            return CreatedAtAction(nameof(GetPartnerLimitAsync), new {id = partner.Id, limitId = newLimit.Id}, null);
        }

		/// <summary>
		/// Get Partner Limit
		/// </summary>
		/// <param name="id">Partner Id, <example>20d2d612-db93-4ed5-86b1-ff2413bca655</example></param>
		/// <param name="limitId">Limit Id, <example>93f3a79d-e9f9-47e6-98bb-1f618db43230</example></param>
		[HttpGet("{id:guid}/limits/{limitId:guid}")]
        public async Task<ActionResult<PartnerPromoCodeLimit>> GetPartnerLimitAsync(ObjectId id, ObjectId limitId)
        {
            var partner = await _partnersRepository.GetByIdAsync(id);

            if (partner == null)
                return NotFound();
            
            var limit = partner.PartnerLimits
                .FirstOrDefault(x => x.Id == limitId);

            var response = new PartnerPromoCodeLimitResponse()
            {
                Id = limit.Id,
                PartnerId = limit.PartnerId,
                Limit = limit.Limit,
                CreateDate = limit.CreateDate.ToString("dd.MM.yyyy hh:mm:ss"),
                EndDate = limit.EndDate.ToString("dd.MM.yyyy hh:mm:ss"),
                CancelDate = limit.CancelDate?.ToString("dd.MM.yyyy hh:mm:ss"),
            };
            
            return Ok(response);
        }

		/// <summary>
		/// Cancel Partner Promo Code Limit
		/// </summary>
		/// <param name="id">partner Id, <example>0da65561-cf56-4942-bff2-22f50cf70d43</example></param>
		[HttpPost("{id:guid}/canceledLimits")]
        public async Task<IActionResult> CancelPartnerPromoCodeLimitAsync(ObjectId id)
        {
            var partner = await _partnersRepository.GetByIdAsync(id);
            
            if (partner == null)
                return NotFound();
            
            if (!partner.IsActive)
                return BadRequest("Partner is unactive");
            
            var activeLimit = partner.PartnerLimits.FirstOrDefault(x => 
                !x.CancelDate.HasValue);
            
            if (activeLimit != null)
            {
                activeLimit.CancelDate = DateTime.Now;
            }

            await _partnersRepository.UpdateAsync(partner);

            await _notificationGateway
                .SendNotificationToPartnerAsync(partner.Id, "Your limit canceled");
            
            return NoContent();
        }

		/// <summary>
		/// Get Partner Promo Codes 
		/// </summary>
		/// <returns></returns>
		[HttpGet("{id:guid}/promocodes")]
        public async Task<IActionResult> GetPartnerPromoCodesAsync(ObjectId id)
        {
            var partner = await _partnersRepository.GetByIdAsync(id);
            
            if (partner == null)
            {
                return NotFound("Parner is null");
            }
            
            var response = partner.PromoCodes
                .Select(x => new PromoCodeShortResponse()
            {
                Id = x.Id,
                Code = x.Code,
                BeginDate = x.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                PartnerName = x.Partner.Name,
                PartnerId = x.PartnerId,
                ServiceInfo = x.ServiceInfo
            }).ToList();

            return Ok(response);
        }

		/// <summary>
		/// Get Partner PromoCode
		/// </summary>
		/// <returns></returns>
		[HttpGet("{id:guid}/promocodes/{promoCodeId:guid}")]
        public async Task<IActionResult> GetPartnerPromoCodeAsync(ObjectId id, ObjectId promoCodeId)
        {
            var partner = await _partnersRepository.GetByIdAsync(id);
            
            if (partner == null)
            {
                return NotFound("Parner is null");
            }

            var promoCode = partner.PromoCodes.FirstOrDefault(x => x.Id == promoCodeId);

            if (promoCode == null)
            {
                return NotFound("Parner is null");
            }
            
            var response =  new PromoCodeShortResponse()
                {
                    Id = promoCode.Id,
                    Code = promoCode.Code,
                    BeginDate = promoCode.BeginDate.ToString("yyyy-MM-dd"),
                    EndDate = promoCode.EndDate.ToString("yyyy-MM-dd"),
                    PartnerName = promoCode.Partner.Name,
                    PartnerId = promoCode.PartnerId,
                    ServiceInfo = promoCode.ServiceInfo
                };

            return Ok(response);
        }
        
        /// <summary>
        /// Create PromoCode
        /// </summary>
        /// <param name="id">Partner Id, <example>20d2d612-db93-4ed5-86b1-ff2413bca655</example></param>
        /// <param name="request">Данные запроса/example></param>
        /// <returns></returns>
        [HttpPost("{id:guid}/promocodes")]
        public async Task<IActionResult> ReceivePromoCodeFromPartnerWithPreferenceAsync(ObjectId id,
            ReceivingPromoCodeRequest request)
        {
            var partner = await _partnersRepository.GetByIdAsync(id);
            
            if (partner == null)
            {
                return BadRequest("Parner is null");
            }

            var activeLimit = partner.PartnerLimits.FirstOrDefault(x
                => !x.CancelDate.HasValue && x.EndDate > DateTime.Now);

            if (activeLimit == null)
            {
                return BadRequest("There is no limit available for providing promotional codes");
            }

            if (partner.NumberIssuedPromoCodes + 1 > activeLimit.Limit)
            {
                return BadRequest("The limit for issuing promotional codes has been exceeded");
            }

            if (partner.PromoCodes.Any(x => x.Code == request.PromoCode))
            {
                return BadRequest("This promotional code has already been issued previously");
            }

            var preference = await _preferencesRepository.GetByIdAsync(request.PreferenceId);

            if (preference == null)
            {
                return BadRequest("Preference not found");
            }

            PromoCode promoCode = PromoCodeMapper.MapFromModel(request, preference, partner);
            partner.PromoCodes.Add(promoCode);
            partner.NumberIssuedPromoCodes++;

            await _partnersRepository.UpdateAsync(partner);
            
            await _givingPromoCodeToCustomerGateway.GivePromoCodeToCustomer(promoCode);


            if (request.PartnerManagerId.HasValue)
            {
                await _administrationGateway.NotifyAdminAboutPartnerManagerPromoCode(request.PartnerManagerId.Value);   
            }

            return CreatedAtAction(nameof(GetPartnerPromoCodeAsync), 
                new {id = partner.Id, promoCodeId = promoCode.Id}, null);
        }
    }
}