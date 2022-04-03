using Domain.DomainExceptions;
using Domain.Entities;
using Domain.Interfaces;
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
    public class AuditServiceTest
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
            _lockService = new LockService(_unitOfWork, _auditService);
            _userService = new UserService(_unitOfWork);

        }

        [Test]
        public void  RegisterAudit_NullAudit_Throws_AuditInvalidException()
        {
      
            //Assert         
            Assert.ThrowsAsync<AuditInvalidException>(() => _auditService.RegisterAudit(null));
        }

        [Test]
        public async Task RegisterAudit_SameAuditTwice_Throws_AuditInvalidException()
        {
            //Arrange
            var audit = new Audit("auditObjName", "auditObjId", "columnChanged", "columnNewValue");

            //Act
            await _auditService.RegisterAudit(audit);
            await _unitOfWork.SaveChangesAsync();

            //Assert         
            Assert.ThrowsAsync<AuditInvalidException>(() => _auditService.RegisterAudit(audit));
        }

        [Test]
        public void GetAuditDetails_NullAuditObjectId_Throws_AuditInvalidException()
        {
           
            //Assert         
            Assert.ThrowsAsync<AuditInvalidException>(() => _auditService.GetAuditDetails(null));
        }
        [Test]
        public void GetAuditDetails_NotExistingAudit_Throws_AuditInvalidException()
        {
          
            //Assert         
            Assert.ThrowsAsync<AuditInvalidException>(() => _auditService.GetAuditDetails("fakeAuditObjId"));
        }

        [Test]
        public async Task GetAuditDetails_ValidAudit_Returns_AuditDetails()
        {
            //Arrange
            var audit = new Audit("auditObjName", "auditObjId", "columnChanged", "columnNewValue");

            //Act
            await _auditService.RegisterAudit(audit);
            await _unitOfWork.SaveChangesAsync();
            var auditDetails = await _auditService.GetAuditDetails(audit.AuditObjectId);
            //Assert         
            Assert.IsTrue(auditDetails != null);
            Assert.IsTrue(auditDetails.Count() == 1);
            Assert.IsTrue(auditDetails.FirstOrDefault().AuditId == audit.AuditId);
            Assert.IsTrue(auditDetails.FirstOrDefault().AuditObjectId == audit.AuditObjectId);

        }
    }
}
