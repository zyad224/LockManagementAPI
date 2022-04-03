using Domain.Base;
using Domain.DomainExceptions;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
   public class Audit: BaseEntity
    {
        [Key]
        [Required(ErrorMessage = "AuditId is Required"), MaxLength(60)]
        public string AuditId { get; private set; }
        public string AuditObjectId { get; private set; }    // PK for Object(Lock,User,etc)

        public string AuditObjectName { get; private set; }

        public string ColumnChanged { get; private set; }
        public string ColumnNewValue { get; private set; }

        private Audit()
        {

        }

        public Audit(string auditObjectName, string auditObjectId, string columnChanged, string columnNewValue)
        {
            if ((string.IsNullOrEmpty(auditObjectName)) ||
                (string.IsNullOrEmpty(auditObjectId)) ||
                (string.IsNullOrEmpty(columnChanged)) ||
                (string.IsNullOrEmpty(columnNewValue)))
                throw new AuditInvalidException("AuditInvalidException");

            AuditId = UUIDGenerator.NewUUID();
            AuditObjectName = auditObjectName;
            AuditObjectId = auditObjectId;
            ColumnChanged = columnChanged;
            ColumnNewValue = columnNewValue;      
            CreatedOn = DateTime.UtcNow;
            ModifiedOn = DateTime.UtcNow;
        }



    }
}
