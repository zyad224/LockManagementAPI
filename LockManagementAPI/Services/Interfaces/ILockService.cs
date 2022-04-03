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
        public void LockCommand(Lock lockk, bool command);
        public Task RegisterLock(Lock lockk);
        public Task<Lock> GetLockById(string lockId);

    }
}
