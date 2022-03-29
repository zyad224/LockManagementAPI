using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shared
{
    public static class UUIDGenerator
    {
        public static string NewUUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
