using Domain.Base;
using Domain.DomainEvents;
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
        [Required(ErrorMessage = "HardwareId is Required"), MaxLength(60)]
        public string HardwareId { get; private set; }

        [ForeignKey("UserId")]
        [Required(ErrorMessage = "UserId is Required"), MaxLength(60)]
        public string UserId { get; private set; }
        public virtual User User { get; private set; }

        public event EventHandler<DomainEventArgs> LockCommandChanged;
        [NotMapped]
        private static readonly object _lock = new object();


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
                    ModifiedOn = DateTime.UtcNow;
                    OnLockCommandChanged();                     
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

        private void OnLockCommandChanged()
        {
            var eventArgs = new DomainEventArgs
            {
               DomainObject = this.GetType().ToString(),
               DomainObjectId = this.LockId,
               ChangedDomainPropertyName = nameof(LockCommand),
               ChangedDomainPropertyNewValue = LockCommand.ToString()
            };

            LockCommandChanged?.Invoke(this, eventArgs);
        }
    }
}
