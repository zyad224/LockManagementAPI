using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockManagementAPI.Dtos.AuditsDto
{
    public class AuditRespDto
    {
        public string AuditObjectId { get; set; }
        public string AuditObjectName { get; set; }

        public string ColumnChanged { get; set; }
        public string ColumnNewValue { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
