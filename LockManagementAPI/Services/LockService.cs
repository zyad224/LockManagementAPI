using Domain.DomainEvents;
using Domain.DomainExceptions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockManagementAPI.Services.Interfaces
{
    public class LockService : ILockService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditService _auditService;

        public LockService(IUnitOfWork unitOfWork, IAuditService auditService)
        {
            _unitOfWork = unitOfWork;
            _auditService = auditService;
        }

        public async Task<Lock> GetLockById(string lockId)
        {
            return await _unitOfWork.LockRepo.GetLockById(lockId);
        }

        public async Task RegisterLock(Lock lockk)
        {
            await _unitOfWork.LockRepo.RegisterLock(lockk);
        }

        public void LockCommand(Lock lockk, bool command)
        {
            if(lockk == null)
                throw new LockInvalidException("LockDoesNotExistException");
            lockk.LockCommandChanged -= lockk_LockCommandChanged;
            lockk.LockCommandChanged += lockk_LockCommandChanged;
            lockk.SetLockCommand(command);
        }

        private void lockk_LockCommandChanged(object sender, DomainEventArgs eventArgs)
        {
            var lockk = (Lock)sender;
            var audit = new Audit(lockk.GetType().ToString(), lockk.LockId, eventArgs.ChangedDomainPropertyName, eventArgs.ChangedDomainPropertyNewValue);
            _auditService.RegisterAudit(audit);
        }

      
    }
}
