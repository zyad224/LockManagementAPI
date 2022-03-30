using Domain.ValueObjects;
using LockManagementAPI.Dtos.LocksDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockManagementAPI.Dtos.UserDtos
{
    public class UserRegisterRespDto
    {
        public string UserId { get; set; }

        public Name Name { get; set; }

        public Email Email { get; set; }
        public string Role { get; set; }


    }
}
