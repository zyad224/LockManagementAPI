using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        public ILockAuditRepository LockAuditRepo { get; }
        public ILockRepository LockRepo { get; }
        public IUserRepository UserRepo { get; }
        public Task<int> SaveChangesAsync();

    }
}
