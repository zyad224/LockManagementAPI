using Domain.Base;
using Domain.DomainExceptions;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class Lock : BaseEntity
    {
        [Key]
        [Required(ErrorMessage = "LockId is Required"), MaxLength(60)]
        public string LockId { get; private set; }
        public string Description { get; private set; }
        public bool LockCommand { get; private set; }
        public string HardwareId { get; private set; }

        [ForeignKey("UserId")]
        [Required(ErrorMessage = "UserId is Required"), MaxLength(60)]
        public string UserId { get; private set; }
        public virtual User User { get; private set; }
        public virtual IEnumerable<LockAudit> LockAudits { get; }


        public event EventHandler LockCommandChanged;
        [NotMapped]
        private object _lock;


        private Lock()
        {

        }
        public Lock(string userId, string description, string hardwareId)
        {
            if ((string.IsNullOrEmpty(description)) ||
                (string.IsNullOrEmpty(userId)) ||
                (string.IsNullOrEmpty(hardwareId)))
                throw new LockInvalidException("Lock Invalid Exception");

            LockId = UUIDGenerator.NewUUID();
            Description = description;
            HardwareId = hardwareId;
            UserId = userId;
            CreatedOn = DateTime.UtcNow;
            ModifiedOn = DateTime.UtcNow;
        }

        public void SetLockCommand(bool lockCommand)
        {
            lock (_lock)
            {
                if(lockCommand != LockCommand)
                {
                    LockCommand = lockCommand;
                    OnLockCommandChanged(EventArgs.Empty);
                    ModifiedOn = DateTime.UtcNow;

                }
            }
            

        }
        public void SetLockDesctiption(string description)
        {
            if (string.IsNullOrEmpty(description))              
                throw new LockInvalidException("Lock Invalid Exception");
            Description = description;
            ModifiedOn = DateTime.UtcNow;

        }
        public void SetHardwareId(string hardwareId)
        {
            if (string.IsNullOrEmpty(hardwareId))
                throw new LockInvalidException("Lock Invalid Exception");
            HardwareId = hardwareId;
            ModifiedOn = DateTime.UtcNow;

        }

        protected virtual void OnLockCommandChanged(EventArgs e)
        {
            LockCommandChanged?.Invoke(this, e);
        }
    }
}
