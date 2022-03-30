using Domain.Entities;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockManagementAPI.Services.Interfaces
{
    public interface ILockService
    {     
        public Task<(Lock lockk,Command oldLockCommand, Command currentLockCommand)> LockCommand(Lock lockk, Command newLockCommand);
        public Task RegisterLock(Lock lockk);
        public Task<Lock> GetLockById(string lockId);
        public Task<List<LockAudit>> GetLockAudits(string lockId, int pageNumber = 1, int pageSize = 10);
        public Task UpdateLockDetails(Lock lockk);

    }
}
