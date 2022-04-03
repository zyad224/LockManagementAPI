using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DomainExceptions
{
    public class NameInvalidException: Exception
    {
        public NameInvalidException(string message)
        : base(message)
        {
        }
    }
}
