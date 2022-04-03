using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LockManagementAPI.Dtos.AuditsDto
{
    public class AuditReqDto
    {
        [Required(ErrorMessage = "AuditObjectId is Required"), MaxLength(60)]
        public string AuditObjectId { get; set; }    // PK for Object(Lock,User,etc)
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }
}
