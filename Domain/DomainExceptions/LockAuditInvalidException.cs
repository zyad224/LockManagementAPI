using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DomainExceptions
{
    public class LockAuditInvalidException:Exception
    {
        public LockAuditInvalidException(string message)
       : base(message)
        {
        }
    }
}
