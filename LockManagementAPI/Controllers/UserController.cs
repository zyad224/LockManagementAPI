using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using LockManagementAPI.Dtos.UserDtos;
using LockManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockManagementAPI.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUserService userService, IJwtService jwtService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _jwtService = jwtService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<UserRegisterRespDto>> Register([FromBody] UserRegisterReqDto userRegisterReqDto)
        {

            var user = _mapper.Map<User>(userRegisterReqDto);
            await _userService.RegisterUser(user);
            await _unitOfWork.SaveChangesAsync();
            var userRegisterRespDto = _mapper.Map<UserRegisterRespDto>(user);
            return Ok(userRegisterRespDto);

        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<UserLoginRespDto>> Login([FromBody] UserLoginReqDto userLoginReqDto)
        {

            var user = await _userService.AuthenticateUser(new Email(userLoginReqDto.Email), userLoginReqDto.Password);
            var jwt = _jwtService.GenerateJWT(user);
            user.SetUserJwt(jwt);
            var userLoginRespDto = _mapper.Map<UserLoginRespDto>(user);
            return Ok(userLoginRespDto);

        }
    }
}
