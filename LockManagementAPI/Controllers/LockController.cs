using AutoMapper;
using Domain.DomainExceptions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Shared;
using LockManagementAPI.Dtos.LocksDto;
using LockManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockManagementAPI.Controllers
{
    [Route("api/Lock")]
    [ApiController]
    public class LockController : ControllerBase
    {

        private readonly ILockService _lockService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public LockController(ILockService lockService,IUserService userService,IMapper mapper, IUnitOfWork unitOfWork)
        {
            _lockService = lockService;
            _userService = userService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Add Lock (For Admins with JWTs ONLY).
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/Lock/AddLock
        ///     {        
        ///       "Description": "This lock for company door",
        ///       "HardwareId": "123",
        ///       "UserId":"10010"
        ///     }
        /// </remarks>
        /// <returns> LockAddRespDto </returns>
        /// /// <response code="200"> LockAddRespDto </response>
        /// <response code="400">LockInvalidException</response> 
        /// <response code="401">Unauthorized</response> 
        /// <response code="403">Forbidden</response> 
        // POST: api/Lock/AddLock
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost]
        [Route("AddLock")]
        public async Task<ActionResult<LockAddRespDto>> AddLock([FromBody] LockAddReqDto lockAddReqDto)
        {
            await _userService.GetUserById(lockAddReqDto.UserId);
            var lockk = _mapper.Map<Lock>(lockAddReqDto);
            await _lockService.RegisterLock(lockk);
            await _unitOfWork.SaveChangesAsync();
            var lockResponseDtoDto = _mapper.Map<LockAddRespDto>(lockk);
            return Ok(lockResponseDtoDto);

        }

        /// <summary>
        /// LockCommand (For ALL Users with JWTs).
        /// </summary>
        /// <remarks>
        /// Sample request: 
        /// 
        ///     POST api/Lock/LockCommand
        ///     {        
        ///       "LockId": "101",
        ///       "UserId": "10010",
        ///       "LockCommand":"true"
        ///     }
        /// </remarks>
        /// 
        /// /// <response code="200"> Successfully Locked/Unlocked </response>
        /// <response code="400">LockInvalidException</response> 
        /// <response code="401">Unauthorized</response> 
        /// <response code="404">LockNotFoundException</response> 
        // POST: api/Lock/LockCommand
        [Authorize]
        [HttpPost]
        [Route("LockCommand")]
        public async Task<ActionResult<LockCommandReqDto>> LockCommand([FromBody] LockCommandReqDto lockCommandReqDto)
        {
            var lockk =  await _lockService.GetLockById(lockCommandReqDto.LockId);

            if(lockk.UserId != lockCommandReqDto.UserId)
                throw new LockInvalidException("LockIsNotRelatedToUser");

            _lockService.LockCommand(lockk, lockCommandReqDto.LockCommand);
            await _unitOfWork.SaveChangesAsync();
            return Ok();

        }
    }
}
