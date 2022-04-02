using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using LockManagementAPI.Dtos.AuditsDto;
using LockManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockManagementAPI.Controllers
{
    [Route("api/Audit")]
    [ApiController]
    public class AuditController : ControllerBase
    {

        private readonly IAuditService _auditService;
        private readonly IMapper _mapper;


        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet]
        [Route("Audits")]
        public async Task<ActionResult<List<Audit>>> Audits([FromQuery] AuditReqDto auditReqDto)
        {
            var audits = await _auditService.GetAuditDetails(auditReqDto.AuditObjectId,auditReqDto.PageNumber,auditReqDto.PageSize);
            return Ok(audits);
        }
    }
}
