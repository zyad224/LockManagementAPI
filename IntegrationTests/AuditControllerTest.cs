using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using LockManagementAPI.Controllers;
using LockManagementAPI.Dtos.AuditsDto;
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
    public class AuditControllerTest
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
        public async Task GetAuditsForLockActivity_LockBecame_Unlock_Lock_Unlock_Returns_OrderedAuditList()
        {

            //Arrange
            var userController = new UserController(_userService, _jwtService, _mapper, _unitOfWork);
            var lockController = new LockController(_lockService, _userService, _mapper, _unitOfWork);
            var auditController = new AuditController(_auditService);

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

            var lockAddResult = await lockController.AddLock(lockk);

            LockCommandReqDto lockCommandReqDto = new LockCommandReqDto
            {
                LockId = (((lockAddResult.Result as ObjectResult).Value) as LockAddRespDto).LockId,
                UserId = userRegisterRespDto.UserId,
                LockCommand = true
            };

            await lockController.LockCommand(lockCommandReqDto);    // unlock

            lockCommandReqDto.LockCommand = false;
            await lockController.LockCommand(lockCommandReqDto);    // lock

            lockCommandReqDto.LockCommand = true;
            await lockController.LockCommand(lockCommandReqDto);  // unlock

            AuditReqDto auditReqDto = new AuditReqDto
            {
                AuditObjectId = lockCommandReqDto.LockId
            };
            var result = await auditController.Audits(auditReqDto);
            var auditsResult = ((result.Result as ObjectResult).Value) as List<Audit>;

            //Assert
            Assert.IsTrue(auditsResult.Count == 3);

        }
    }
}
