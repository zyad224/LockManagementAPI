using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DomainExceptions
{
    public class EmailInvalidException: Exception
    {
        public EmailInvalidException(string message)
        : base(message)
        {
        }
    }
}
