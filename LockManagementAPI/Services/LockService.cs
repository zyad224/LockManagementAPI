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

        public LockService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Lock> GetLockById(string lockId)
        {
            return await _unitOfWork.LockRepo.GetLockById(lockId);
        }

        public async Task RegisterLock(Lock lockk)
        {
            await _unitOfWork.LockRepo.RegisterLock(lockk);
        }

        public async Task UpdateLockDetails(Lock lockk)
        {
            await _unitOfWork.LockRepo.UpdateLockDetails(lockk);
        }

        public async Task<(Lock lockk, Command oldLockCommand, Command currentLockCommand)> LockCommand(Lock lockk, Command newLockCommand)
        {
            if(lockk == null)
                throw new LockInvalidException("LockInvalidException");

            if(await _unitOfWork.LockRepo.GetLockById(lockk.LockId) == null)
                throw new LockInvalidException("LockDoesNotExistException");

            var oldLockCommand = lockk.LockCommand;

            lockk.LockCommandChanged += lockk_LockCommandChanged;

            if (newLockCommand == Command.Lock)
                lockk.SetLockCommand(newLockCommand.GetCommand());

            else if (newLockCommand == Command.Unlock)
                lockk.SetLockCommand(newLockCommand.GetCommand());

            return (lockk,
                   (oldLockCommand == true? Command.Unlock: Command.Lock),
                   (lockk.LockCommand == true ? Command.Unlock : Command.Lock));
        }

        public async Task<List<LockAudit>> GetLockAudits(string lockId, int pageNumber = 1, int pageSize = 10)
        {
            return await _unitOfWork.LockAuditRepo.GetLockAuditsByLockId(lockId, pageNumber, pageSize);
        }

        private void lockk_LockCommandChanged(object sender, EventArgs e)
        {
           // can call another services (SQS, SNS, Lambda)
        }

      
    }
}
