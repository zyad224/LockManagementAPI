using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DomainExceptions
{
    public class UserInvalidException:Exception
    {
        public UserInvalidException(string message)
        : base(message)
        {
        }
    }
}
