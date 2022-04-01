using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockManagementAPI.Services.Interfaces
{
    public interface IAuditService
    {

        public Task RegisterAudit(Audit audit);
        public Task<List<Audit>> GetAuditDetails(string auditObjectId, int pageNumber = 1, int pageSize = 10);

    }
}
