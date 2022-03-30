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
    public class LockAuditRepository : ILockAuditRepository
    {
        private readonly DbApiContext _dbApiContext;

        public LockAuditRepository(DbApiContext dbApiContext)
        {
            _dbApiContext = dbApiContext;

        }
        public async Task<List<LockAudit>> GetLockAuditsByLockId(string lockId, int pageNumber = 1, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(lockId))
                throw new LockAuditInvalidException("LockIdInvalidException");

            var lockAudits = await _dbApiContext.LockAudits
           .Include(la=> la.User)
           .Include(la=>la.Lock)
           .Where(la => la.LockId == lockId)
           .OrderBy(la => la.CreatedOn)
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();

            if(lockAudits == null)
                new LockAuditInvalidException("NoLockAuditsForThisLockInvalidException");

            return lockAudits;


        }

        public async Task RegisterLockAudit(LockAudit lockAudit)
        {
            if(lockAudit == null)
                throw new LockAuditInvalidException("LockAuditInvalidException");

            var lockAuditdb = await _dbApiContext.LockAudits.Where(la => la.LockAuditId == lockAudit.LockAuditId).FirstOrDefaultAsync();

            if (lockAuditdb != null)
                throw new LockAuditInvalidException("LockAuditAlreadyExistException");

            await _dbApiContext.LockAudits.AddAsync(lockAudit);

        }
    }
}
