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
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Services
{
    public class UserServiceTest
    {

        private DbApiContext _dbApiContext;
        private IUserService _userService;
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
            _userService = new UserService(_unitOfWork);


           
        }

        [Test]
        public async Task AuthenticateUser_NotExistingUser_Throws_UserNotFoundException()
        {

            //Arrange
            var email = new Email("z@gmail.com");
            var name = new Name("Zeyad", "Ab");
            var user = new User(name, email, "Admin", "123");


            //Act
            await _userService.RegisterUser(user);
            await _unitOfWork.SaveChangesAsync();
            var fakeEmail = new Email("fake@gmail.com");
            var fakeName = new Name("fakeFirst","fakeSecond");
            var fakeUser = new User(fakeName, fakeEmail, "Admin", "123");

            //Assert         
            Assert.ThrowsAsync<UserNotFoundException>(() => _userService.AuthenticateUser(fakeUser.Email, fakeUser.Password));
                      
        }
        [Test]
        public async Task AuthenticateUser_ExistingUser_Returns_User()
        {

            //Arrange
            var email = new Email("z@gmail.com");
            var name = new Name("Zeyad", "Ab");
            var user = new User(name, email, "Admin", "123");


            //Act
            await _userService.RegisterUser(user);
            await _unitOfWork.SaveChangesAsync();
            var authenticatedUser = await _userService.AuthenticateUser(user.Email, user.Password);


            //Assert
            Assert.IsTrue(authenticatedUser != null);
            Assert.IsTrue(authenticatedUser.Email == user.Email);
            Assert.IsTrue(authenticatedUser.Password == user.Password);


        }

        [Test]
        public async Task Register_SameUserTwice_Throws_UserInvalidException()
        {

            //Arrange
            var email = new Email("z@gmail.com");
            var name = new Name("Zeyad", "Ab");
            var user = new User(name, email, "Admin", "123");

            //Act
            await _userService.RegisterUser(user);
            await _unitOfWork.SaveChangesAsync();

            //Assert
            Assert.ThrowsAsync<UserInvalidException>(() => _userService.RegisterUser(user));

        }
        [Test]
        public void Register_NullUser_Throws_UserInvalidException()
        {
        
            //Assert
            Assert.ThrowsAsync<UserInvalidException>(() => _userService.RegisterUser(null));

        }

        [Test]
        public void GetUserById_NullUserId_Throws_UserInvalidException()
        {

            //Assert
            Assert.ThrowsAsync<UserInvalidException>(() => _userService.GetUserById(null));

        }
        [Test]
        public async Task GetUserById_NotExistingUser_Throws_UserNotFoundException()
        {
            //Arrange
            var email = new Email("z@gmail.com");
            var name = new Name("Zeyad", "Ab");
            var user = new User(name, email, "Admin", "123");

            //Act
            await _userService.RegisterUser(user);
            await _unitOfWork.SaveChangesAsync();

            //Assert
            Assert.ThrowsAsync<UserNotFoundException>(() => _userService.GetUserById("fakeUserId"));

        }
        [Test]
        public async Task GetUserById_ExistingUser_Return_User()
        {
            //Arrange
            var email = new Email("z@gmail.com");
            var name = new Name("Zeyad", "Ab");
            var user = new User(name, email, "Admin", "123");

            //Act
            await _userService.RegisterUser(user);
            await _unitOfWork.SaveChangesAsync();

            //Assert
            var existingUser = await _userService.GetUserById(user.UserId);
            Assert.IsTrue(existingUser != null);
            Assert.IsTrue(existingUser.UserId == user.UserId);

        }
    }
}
