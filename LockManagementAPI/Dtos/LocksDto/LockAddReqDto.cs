using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LockManagementAPI.Dtos.LocksDto
{
    public class LockAddReqDto
    {
        public string Description { get; set; }
        [Required(ErrorMessage = "HardwareId is Required"), MaxLength(60)]
        public string HardwareId { get; set; }
        [Required(ErrorMessage = "UserId is Required"), MaxLength(60)]
        public string UserId { get; set; }
      
    }
}
