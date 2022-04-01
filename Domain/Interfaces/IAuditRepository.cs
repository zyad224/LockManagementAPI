using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAuditRepository
    {
        public Task RegisterAudit(Audit audit);
        public Task<List<Audit>> GetAuditDetails(string auditObjectId, int pageNumber = 1,int pageSize = 10);


    }
}
