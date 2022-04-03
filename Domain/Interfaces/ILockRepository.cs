using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILockRepository
    {
        public Task RegisterLock(Lock lockk);
        public Task<Lock> GetLockById(string lockId);

    }
}
