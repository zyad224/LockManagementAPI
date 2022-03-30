using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shared
{
    public static class CommandExtension
    {

        public static bool GetCommand(this Command command)
        {
            if (command == Command.Lock)
                return false;

            if (command == Command.Unlock)
                return true;

            return false;
        }
    }
}
