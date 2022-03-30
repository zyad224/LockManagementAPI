using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockManagementAPI.Dtos.LocksDto
{
    public class LockResponseDto
    {
        public string LockId { get; set; }
        public string Description { get; set; }
        public bool LockCommand { get; set; }
        public string HardwareId { get; set; }     
        public string UserId { get; set; }
    }
}
