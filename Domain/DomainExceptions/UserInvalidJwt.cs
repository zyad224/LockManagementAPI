using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DomainExceptions
{
    public class UserInvalidJwt:Exception
    {
        public UserInvalidJwt(string message)
        : base(message)
        {
        }
    }
}
