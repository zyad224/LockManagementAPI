using AutoMapper;
using Domain.DomainExceptions;
using Domain.Interfaces;
using Domain.Shared;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using LockManagementAPI.Controllers;
using LockManagementAPI.Dtos.UserDtos;
using LockManagementAPI.Profiles;
using LockManagementAPI.Services;
using LockManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class UserControllerTest
    {
        private DbApiContext _dbApiContext;
        private IUserService _userService;
        private IUnitOfWork _unitOfWork;
        private IAuditRepository _auditRepo;
        private IUserRepository _userRepo;
        private ILockRepository _lockRepo;
        private IJwtService _jwtService;
        private IConfiguration _configuration;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<DbApiContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;

            _configuration = new ConfigurationBuilder()
                       .AddJsonFile("appsettingstest.json")
                       .Build();

            var configMapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());

            _mapper = configMapper.CreateMapper();
            _dbApiContext = new DbApiContext(options);
            _userRepo = new UserRepository(_dbApiContext);
            _auditRepo = new AuditRepository(_dbApiContext);
            _lockRepo = new LockRepository(_dbApiContext);
            _unitOfWork = new UnitOfWork(_dbApiContext, _auditRepo, _lockRepo, _userRepo);
            _jwtService = new JwtService(_configuration);
            _userService = new UserService(_unitOfWork);

        }

        [Test]
        public async Task RegisterEndpoint_ValidUser_Returns_UserRegisterRespDto_OK()
        {

            //Arrange
            var userController = new UserController(_userService, _jwtService, _mapper, _unitOfWork);
            UserRegisterReqDto userRegisterReqDto = new UserRegisterReqDto
            {
                FirstName = "Zeyad",
                LastName = "Ab",
                Email = "z@gmail.com",
                Password ="123"
            };
            //Act
            var result = await userController.Register(userRegisterReqDto);
            var resultStatusCode = (result.Result as ObjectResult).StatusCode;
            var resultResponse = ((result.Result as ObjectResult).Value) as UserRegisterRespDto;

            //Assert
            Assert.IsTrue(resultStatusCode == 200);
            Assert.IsTrue(resultResponse != null);
            Assert.IsTrue(resultResponse.UserId != null);
            Assert.IsTrue(resultResponse.Role == UserRole.User);

        }

        [Test]
        public void RegisterEndpoint_InvalidUser_Throws_UserInvalidException_BadRequest()
        {

            //Arrange
            var userController = new UserController(_userService, _jwtService, _mapper, _unitOfWork);
            UserRegisterReqDto userRegisterReqDto = new UserRegisterReqDto
            {
                FirstName = "Zeyad",
                LastName = "Ab",
                Email = "z@gmail.com",
                Password = ""
            };        
            //Assert
            Assert.ThrowsAsync<UserInvalidException>(() => userController.Register(userRegisterReqDto));

        

        }

        [Test]
        public async Task LoginEndpoint_ValidUser_Returns_UserLoginRespDto_OK()
        {

            //Arrange
            var userController = new UserController(_userService, _jwtService, _mapper, _unitOfWork);
            UserRegisterReqDto userRegisterReqDto = new UserRegisterReqDto
            {
                FirstName = "Zeyad",
                LastName = "Ab",
                Email = "z@gmail.com",
                Password = "123"
            };

            UserLoginReqDto userLoginReqDto = new UserLoginReqDto
            {
                Email = "z@gmail.com",
                Password = "123"
            };

            //Act
            await userController.Register(userRegisterReqDto);
            var result = await userController.Login(userLoginReqDto);
            var resultStatusCode = (result.Result as ObjectResult).StatusCode;
            var resultResponse = ((result.Result as ObjectResult).Value) as UserLoginRespDto;

            //Assert
            Assert.IsTrue(resultStatusCode == 200);
            Assert.IsTrue(resultResponse != null);
            Assert.IsTrue(resultResponse.UserId != null);
            Assert.IsFalse(string.IsNullOrEmpty(resultResponse.Jwt));
            Assert.IsTrue(resultResponse.Role == UserRole.User);
        }

        [Test]
        public async Task LoginEndpoint_InValidUser_Returns_Throws_UserNotFoundException_BadRequest()
        {

            //Arrange
            var userController = new UserController(_userService, _jwtService, _mapper, _unitOfWork);
            UserRegisterReqDto userRegisterReqDto = new UserRegisterReqDto
            {
                FirstName = "Zeyad",
                LastName = "Ab",
                Email = "z@gmail.com",
                Password = "123"
            };

            UserLoginReqDto userLoginReqDto = new UserLoginReqDto
            {
                Email = "z@gmail.com",
                Password = "fakePassword"
            };

            //Act
            await userController.Register(userRegisterReqDto);

            //Assert
            Assert.ThrowsAsync<UserNotFoundException>(() => userController.Login(userLoginReqDto));

        }
    }
}
