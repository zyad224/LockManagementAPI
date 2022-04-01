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
        [Authorize]
        [HttpPost]
        [Route("LockCommand")]
        public async Task<ActionResult> LockCommand([FromBody] LockCommandReqDto lockCommandReqDto)
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
