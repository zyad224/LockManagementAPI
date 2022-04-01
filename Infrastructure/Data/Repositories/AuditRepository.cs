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
    public class AuditRepository : IAuditRepository
    {
        private readonly DbApiContext _dbApiContext;

        public AuditRepository(DbApiContext dbApiContext)
        {
            _dbApiContext = dbApiContext;

        }
        public async Task<List<Audit>> GetAuditDetails(string auditObjectId, int pageNumber = 1, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(auditObjectId))
                throw new AuditInvalidException("AuditObjectIdInvalidException");

            var audits = await _dbApiContext.Audits        
           .Where(a => a.AuditObjectId == auditObjectId)
           .OrderByDescending(a => a.CreatedOn)
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();

            if(audits == null)
                new AuditInvalidException("NoAuditsForThisObjectInvalidException");

            return audits;
        }

        public async Task RegisterAudit(Audit audit)
        {
            if(audit == null)
                throw new AuditInvalidException("AuditInvalidException");

            var auditdb = await _dbApiContext.Audits.Where(a => a.AuditId == audit.AuditId).FirstOrDefaultAsync();

            if (auditdb != null)
                throw new AuditInvalidException("AuditAlreadyExistException");

            await _dbApiContext.Audits.AddAsync(audit);

        }
    }
}
