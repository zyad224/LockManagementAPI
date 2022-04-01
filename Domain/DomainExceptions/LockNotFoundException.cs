using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DomainExceptions
{
    public class LockNotFoundException:Exception
    {
        public LockNotFoundException(string message)
        : base(message)
        {
        }
    }
}
