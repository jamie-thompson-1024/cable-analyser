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

        public string Message { get; }
    }
    public class ArduinoMessageSentEventArgs : EventArgs
    {
        public ArduinoMessageSentEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
    public class TestPinConnectionsMessageEventArgs : EventArgs
    {
        public TestPinConnectionsMessageEventArgs(int pin, int[] testedPins)
        {
            Pin = pin;
            TestedPins = testedPins;
        }

        public int Pin { get; }
        public int[] TestedPins { get; }
    }
    public class ErrorMessageEventArgs : EventArgs
    {
        public ErrorMessageEventArgs(ArduinoErrorCode errorCode)
        {
            ErrorCode = errorCode;
        }

        public ArduinoErrorCode ErrorCode { get; }
    }
    public class DeviceTypeMessageEventArgs : EventArgs
    {
        public DeviceTypeMessageEventArgs(string deviceType)
        {
            DeviceType = deviceType;
        }

        public string DeviceType { get; }
    }
}
