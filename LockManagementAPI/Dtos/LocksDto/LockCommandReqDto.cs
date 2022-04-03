using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LockManagementAPI.Dtos.LocksDto
{
    public class LockCommandReqDto
    {
        [Required(ErrorMessage = "LockId is Required"), MaxLength(60)]
        public string LockId { get; set; }
        [Required(ErrorMessage = "UserId is Required"), MaxLength(60)]
        public string UserId { get; set; }
        public bool LockCommand { get; set; }

    }
}
