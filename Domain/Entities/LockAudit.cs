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
   public class LockAudit: BaseEntity
    {
        [Key]
        [Required(ErrorMessage = "LockAuditId is Required"), MaxLength(60)]
        public string LockAuditId { get; private set; }
        public string LockEvent { get; private set; }
        public bool LockCommand { get; private set; }

        [ForeignKey("UserId")]
        [Required(ErrorMessage = "UserId is Required"), MaxLength(60)]
        public string UserId { get; private set; }

        [ForeignKey("LockId")]
        [Required(ErrorMessage = "LockId is Required"), MaxLength(60)]
        public string LockId { get; private set; }

        public virtual User User{ get; private set; }
        public virtual Lock Lock { get; private set; }

        private LockAudit()
        {

        }

        public LockAudit(string lockEvent, bool lockCommand, string userId, string lockId)
        {
            if ((string.IsNullOrEmpty(lockEvent)) ||
                (string.IsNullOrEmpty(userId)) ||
                (string.IsNullOrEmpty(lockId)))
                throw new LockAuditInvalidException("LockAudit Invalid Exception");

            LockAuditId = UUIDGenerator.NewUUID();
            LockEvent = lockEvent;
            LockCommand = lockCommand;
            UserId = userId;
            LockId = lockId;
            CreatedOn = DateTime.UtcNow;
            ModifiedOn = DateTime.UtcNow;
        }



    }
}
