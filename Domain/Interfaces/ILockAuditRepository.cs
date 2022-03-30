using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILockAuditRepository
    {
        public Task RegisterLockAudit(LockAudit lockAudit);
        public Task<List<LockAudit>> GetLockAuditsByLockId(string lockId, int pageNumber = 1,int pageSize = 10);


    }
}
