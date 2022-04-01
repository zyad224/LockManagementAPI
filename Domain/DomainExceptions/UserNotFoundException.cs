using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DomainExceptions
{
    public class UserNotFoundException:Exception
    {
        public UserNotFoundException(string message)
        : base(message)
        {
        }
    }
}
