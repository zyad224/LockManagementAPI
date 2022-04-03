using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DomainExceptions
{
    public class LockInvalidException: Exception
    {
        public LockInvalidException(string message)
       : base(message)
        {
        }
    }
}
