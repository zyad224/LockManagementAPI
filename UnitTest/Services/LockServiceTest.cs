using Domain.DomainExceptions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using LockManagementAPI.Services;
using LockManagementAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Services
{
    public class LockServiceTest
    {
        private DbApiContext _dbApiContext;
        private IUserService _userService;
        private ILockService _lockService;
        private IAuditService _auditService;
        private IUnitOfWork _unitOfWork;
        private IAuditRepository _auditRepo;
        private IUserRepository _userRepo;
        private ILockRepository _lockRepo;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<DbApiContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;

            _dbApiContext = new DbApiContext(options);
            _userRepo = new UserRepository(_dbApiContext);
            _auditRepo = new AuditRepository(_dbApiContext);
            _lockRepo = new LockRepository(_dbApiContext);
            _unitOfWork = new UnitOfWork(_dbApiContext, _auditRepo, _lockRepo, _userRepo);
            _auditService = new AuditService(_unitOfWork);
            _lockService = new LockService(_unitOfWork,_auditService);
            _userService = new UserService(_unitOfWork);

        }

        [Test]
        public async Task RegisterLock_SameLockTwice_Throws_LockInvalidException()
        {

            //Arrange
            var email = new Email("z@gmail.com");
            var name = new Name("Zeyad", "Ab");
            var user = new User(name, email, "Admin", "123");
            var lockk = new Lock(user.UserId, "description", "hardwareId");


            //Act
            await _userService.RegisterUser(user);
            await _lockService.RegisterLock(lockk);
            await _unitOfWork.SaveChangesAsync();
            //Assert         
            Assert.ThrowsAsync<LockInvalidException>(() => _lockService.RegisterLock(lockk));

        }
        [Test]
        public void RegisterLock_NullLock_Throws_LockInvalidException()
        {
          
            //Assert         
            Assert.ThrowsAsync<LockInvalidException>(() => _lockService.RegisterLock(null));

        }

        [Test]
        public async Task RegisterMultipleLocks_ForSameUser_Checks_UserWithLocksList()
        {

            //Arrange
            var email = new Email("z@gmail.com");
            var name = new Name("Zeyad", "Ab");
            var user = new User(name, email, "Admin", "123");
            var lockk1 = new Lock(user.UserId, "description1", "hardwareId1");
            var lockk2 = new Lock(user.UserId, "description2", "hardwareId2");

            //Act
            await _userService.RegisterUser(user);
            await _lockService.RegisterLock(lockk1);
            await _lockService.RegisterLock(lockk2);
            await _unitOfWork.SaveChangesAsync();

            var userdb = await _userService.GetUserById(user.UserId);
            //Assert         
            Assert.IsTrue(userdb.Locks != null);
            Assert.IsTrue(userdb.Locks.ToList().Count == 2);

        }
        [Test]
        public void GetLockById_NullLockId_Throws_LockInvalidException()
        {

            //Assert
            Assert.ThrowsAsync<LockInvalidException>(() => _lockService.GetLockById(null));

        }

        [Test]
        public void  GetLockById_NotExistingLock_Throws_LockNotFoundException()
        {
           
            //Assert
            Assert.ThrowsAsync<LockNotFoundException>(() => _lockService.GetLockById("fakeLockId"));

        }
        [Test]
        public async Task GetLockById_ValidLock_Returns_Lock()
        {

            //Arrange
            var email = new Email("z@gmail.com");
            var name = new Name("Zeyad", "Ab");
            var user = new User(name, email, "Admin", "123");
            var lockk = new Lock(user.UserId, "description1", "hardwareId1");

            //Act
            await _userService.RegisterUser(user);
            await _lockService.RegisterLock(lockk);
            await _unitOfWork.SaveChangesAsync();

            var lockdb = await _lockService.GetLockById(lockk.LockId);
            //Assert         
            Assert.IsTrue(lockdb != null);
            Assert.IsTrue(lockdb.UserId == user.UserId);

        }

        [Test]
        public async Task LockCommand_Unlock_Checks_LockIsUnlocked() // unlock = true, lock = false
        {

            //Arrange
            var email = new Email("z@gmail.com");
            var name = new Name("Zeyad", "Ab");
            var user = new User(name, email, "Admin", "123");
            var lockk = new Lock(user.UserId, "description1", "hardwareId1");

            //Act
            await _userService.RegisterUser(user);
            await _lockService.RegisterLock(lockk);
            await _unitOfWork.SaveChangesAsync();

            //Assert         
            Assert.IsTrue(lockk.LockCommand == false);
            _lockService.LockCommand(lockk, true);
            Assert.IsTrue(lockk.LockCommand == true);

        }

        [Test]
        public void LockCommand_NullLock_Throws_LockInvalidException()
        {

            //Assert
            Assert.Throws<LockInvalidException>(() => _lockService.LockCommand(null,true));

        }
    }
}
