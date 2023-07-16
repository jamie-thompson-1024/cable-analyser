using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConnector
{
    public class ArduinoMessageReceivedEventArgs : EventArgs
    {
        public ArduinoMessageReceivedEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
    public class ArduinoMessageSentEventArgs : EventArgs
    {
        public ArduinoMessageSentEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
    public class TestPinConnectionsMessageEventArgs : EventArgs
    {
        public TestPinConnectionsMessageEventArgs(int pin, int[] testedPins)
        {
            Pin = pin;
            TestedPins = testedPins;
        }

        public int Pin { get; set; }
        public int[] TestedPins { get; set; }
    }
    public class ErrorMessageEventArgs : EventArgs
    {
        public ErrorMessageEventArgs(ArduinoErrorCode errorCode)
        {
            ErrorCode = errorCode;
        }

        public ArduinoErrorCode ErrorCode { get; set; }
    }
    public class DeviceTypeMessageEventArgs : EventArgs
    {
        public DeviceTypeMessageEventArgs(string deviceType)
        {
            DeviceType = deviceType;
        }

        public string DeviceType { get; set; }
    }
}
