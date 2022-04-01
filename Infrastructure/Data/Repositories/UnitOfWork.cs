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
        public IAuditRepository AuditRepo { get; }
        public ILockRepository LockRepo { get; }
        public IUserRepository UserRepo { get; }

        public UnitOfWork(DbApiContext dbApiContext, IAuditRepository auditRepo, ILockRepository lockRepo, IUserRepository userRepo)
        {
            _dbApiContext = dbApiContext;
            AuditRepo = auditRepo;
            LockRepo = lockRepo;
            UserRepo = userRepo;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _dbApiContext.SaveChangesAsync();
        }
   
    }
}
