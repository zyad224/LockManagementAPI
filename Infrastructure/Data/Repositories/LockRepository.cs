using Domain.DomainExceptions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class LockRepository : ILockRepository
    {
        private readonly DbApiContext _dbApiContext;

        public LockRepository(DbApiContext dbApiContext)
        {
            _dbApiContext = dbApiContext;

        }
        public async Task<Lock> GetLockById(string lockId)
        {
            if (string.IsNullOrEmpty(lockId))
                throw new LockInvalidException("LockInvalidException");

            return await _dbApiContext.Locks.Where(l => l.LockId == lockId).FirstOrDefaultAsync();
        }

        public async Task RegisterLock(Lock lockk)
        {
            if (lockk == null)
                throw new LockInvalidException("LockInvalidException");

            var lockdb = await _dbApiContext.Locks.Where(l => l.LockId == lockk.LockId).FirstOrDefaultAsync();

            if (lockdb != null)
                throw new LockInvalidException("LockAlreadyExistException");

            await _dbApiContext.Locks.AddAsync(lockk);
        }

        public async Task UpdateLockDetails(Lock lockk)
        {
            if (lockk == null)
                throw new LockInvalidException("LockInvalidException");

            var lockdb = await _dbApiContext.Locks.Where(l => l.LockId == lockk.LockId).FirstOrDefaultAsync();

            lockdb.SetLockDesctiption(lockk.Description);
            lockdb.SetLockCommand(lockk.LockCommand);
            lockdb.SetHardwareId(lockk.HardwareId);
        }
    }
}
