using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DomainExceptions
{
    public class RoleInvalidException:Exception
    {
        public RoleInvalidException(string message)
       : base(message)
        {
        }
    }
}
