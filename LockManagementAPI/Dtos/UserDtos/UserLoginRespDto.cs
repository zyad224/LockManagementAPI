using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockManagementAPI.Dtos.UserDtos
{
    public class UserLoginRespDto
    {
        public string UserId { get; set; }
        public Email Email { get; set; }
        public string Jwt { get; set; }
        public string Role { get; set; }


    }
}
