using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly DbApiContext _dbApiContext;
        public ILockAuditRepository LockAuditRepo { get; }
        public ILockRepository LockRepo { get; }
        public IUserRepository UserRepo { get; }

        public UnitOfWork(DbApiContext dbApiContext, ILockAuditRepository lockAuditRepo, ILockRepository lockRepo, IUserRepository userRepo)
        {
            _dbApiContext = dbApiContext;
            LockAuditRepo = lockAuditRepo;
            LockRepo = lockRepo;
            UserRepo = userRepo;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _dbApiContext.SaveChangesAsync();
        }
   
    }
}
