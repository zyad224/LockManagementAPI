using Domain.DomainExceptions;
using Domain.Entities;
using Domain.Interfaces;
using LockManagementAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockManagementAPI.Services
{
    public class AuditService : IAuditService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuditService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task RegisterAudit(object obj)
        {
            if (obj == null)
                throw new AuditInvalidException("ObjectToRegisterIsNull");

            if (obj.GetType() == typeof(LockAudit))
                await _unitOfWork.LockAuditRepo.RegisterLockAudit((LockAudit)obj);
        }
    }
}
