using Domain.DomainExceptions;
using Domain.Entities;
using Domain.Interfaces;
using LockManagementAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockManagementAPI.Services
{
    public class AuditService : IAuditService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuditService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Audit>> GetAuditDetails(string auditObjectId, int pageNumber = 1, int pageSize = 10)
        {
            return await _unitOfWork.AuditRepo.GetAuditDetails(auditObjectId, pageNumber, pageSize);
        }

        public async Task RegisterAudit(Audit audit)
        {
            await _unitOfWork.AuditRepo.RegisterAudit(audit);
        }
    }
}
