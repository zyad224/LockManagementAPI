using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DomainExceptions
{
    public class AuditInvalidException: Exception
    {
        public AuditInvalidException(string message)
        : base(message)
        {
        }
    }
}
