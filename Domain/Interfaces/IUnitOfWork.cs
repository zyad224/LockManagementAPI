using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        public IAuditRepository AuditRepo { get; }
        public ILockRepository LockRepo { get; }
        public IUserRepository UserRepo { get; }
        public Task<int> SaveChangesAsync();

    }
}
