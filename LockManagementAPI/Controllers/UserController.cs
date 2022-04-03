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


        /// <summary>
        /// Register New User.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/User/Register
        ///     {        
        ///       "FirstName": "Zyad",
        ///       "LastName":"Ab",
        ///       "Email":"z@gmail.com"
        ///       "Password": "123",
        ///       "Role":"Admin"
        ///     }
        /// </remarks>
        /// <returns> UserRegisterRespDto </returns>
        /// /// <response code="200"> UserRegisterRespDto</response>
        /// <response code="400">UserInvalidException</response> 
        ///
        ///  
        // POST: api/User/Register
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

        /// <summary>
        /// User Login.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/User/Login
        ///     {        
        ///       "Email": "z@gmail.com",
        ///       "Password": "123"
        ///     }
        /// </remarks>
        /// <returns> UserLoginRespDto </returns>
        /// /// <response code="200"> UserLoginRespDto </response>
        /// <response code="400">UserInvalidException</response> 
        /// <response code="404">UserNotFoundException</response> 
        // POST: api/User/Login
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
