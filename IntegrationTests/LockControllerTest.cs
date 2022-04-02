using AutoMapper;
using Domain.DomainExceptions;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using LockManagementAPI.Controllers;
using LockManagementAPI.Dtos.LocksDto;
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
    public class LockControllerTest
    {
        private DbApiContext _dbApiContext;
        private IUserService _userService;
        private ILockService _lockService;
        private IAuditService _auditService;
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
            _auditService = new AuditService(_unitOfWork);
            _lockService = new LockService(_unitOfWork, _auditService);

        }

        [Test]
        public async Task AddLockEndpoint_NotExistingUser_Throws_UserNotFoundException_BadRequest()
        {

            //Arrange
            var userController = new UserController(_userService, _jwtService, _mapper, _unitOfWork);
            var lockController = new LockController(_lockService, _userService, _mapper, _unitOfWork);

            UserRegisterReqDto userRegisterReqDto = new UserRegisterReqDto
            {
                FirstName = "Zeyad",
                LastName = "Ab",
                Email = "z@gmail.com",
                Password = "123"
            };

            LockAddReqDto lockAddReqDto = new LockAddReqDto
            {
                Description = "description",
                HardwareId = "hardwareId",
                UserId = "notExistingUser"
            };


            //Act
            await userController.Register(userRegisterReqDto);

            //Assert
            Assert.ThrowsAsync<UserNotFoundException>(() => lockController.AddLock(lockAddReqDto));
        }

        [Test]
        public async Task AddLockEndpoint_AddMultipleLocks_ForValidUser_Returns_OK()
        {

            //Arrange
            var userController = new UserController(_userService, _jwtService, _mapper, _unitOfWork);
            var lockController = new LockController(_lockService, _userService, _mapper, _unitOfWork);

            UserRegisterReqDto userRegisterReqDto = new UserRegisterReqDto
            {
                FirstName = "Zeyad",
                LastName = "Ab",
                Email = "z@gmail.com",
                Password = "123"
            };

         
            //Act
             var userRegisterResult = await userController.Register(userRegisterReqDto);
             var userRegisterRespDto = ((userRegisterResult.Result as ObjectResult).Value) as UserRegisterRespDto;

            LockAddReqDto lock1 = new LockAddReqDto
            {
                Description = "description1",
                HardwareId = "hardwareId1",
                UserId = userRegisterRespDto.UserId

            };
            LockAddReqDto lock2 = new LockAddReqDto
            {
                Description = "description2",
                HardwareId = "hardwareId2",
                UserId = userRegisterRespDto.UserId

            };

            var lock1AddLockResult = await lockController.AddLock(lock1);
            var lock2AddLockResult = await lockController.AddLock(lock2);

            var lock1ResultStatusCode = (lock1AddLockResult.Result as ObjectResult).StatusCode;
            var lock2ResultStatusCode = (lock2AddLockResult.Result as ObjectResult).StatusCode;


            //Assert
            Assert.IsTrue(lock1ResultStatusCode == 200);
            Assert.IsTrue(lock2ResultStatusCode == 200);

        }

        [Test]
        public async Task LockCommand_SubmitAnotherUserId_Throws_LockInvalidException_BadRequest()
        {

            //Arrange
            var userController = new UserController(_userService, _jwtService, _mapper, _unitOfWork);
            var lockController = new LockController(_lockService, _userService, _mapper, _unitOfWork);

            UserRegisterReqDto userRegisterReqDto = new UserRegisterReqDto
            {
                FirstName = "Zeyad",
                LastName = "Ab",
                Email = "z@gmail.com",
                Password = "123"
            };
       

            //Act
            var userRegisterResult = await userController.Register(userRegisterReqDto);
            var userRegisterRespDto = ((userRegisterResult.Result as ObjectResult).Value) as UserRegisterRespDto;


            LockAddReqDto lockk = new LockAddReqDto
            {
                Description = "description1",
                HardwareId = "hardwareId1",
                UserId = userRegisterRespDto.UserId

            };

            var result= await lockController.AddLock(lockk);

            LockCommandReqDto lockCommandReqDto = new LockCommandReqDto
            {
                LockId =(((result.Result as ObjectResult).Value) as LockAddRespDto).LockId,
                UserId = "anotherUserId",
                LockCommand = true
            };


            //Assert
            Assert.ThrowsAsync<LockInvalidException>(() => lockController.LockCommand(lockCommandReqDto));



        }
    }
}

    


