using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConnector
{
    public enum ArduinoErrorCode
    {
        INVALID_TEST_PIN = 1,
        INVALID_IO_PIN = 2,
        INVALID_COMMAND = 3,
        INVALID_COMMAND_SYNTAX = 4,
    }
}
