using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DomainExceptions
{
    public class PasswordInvalidException:Exception
    {
        public PasswordInvalidException(string message)
       : base(message)
        {
        }
    }
}
