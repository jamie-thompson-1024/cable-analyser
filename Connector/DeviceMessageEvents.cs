using System;

namespace DeviceConnector
{
    public class DeviceMessageReceivedEventArgs : EventArgs
    {
        public DeviceMessageReceivedEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
    public class DeviceMessageSentEventArgs : EventArgs
    {
        public DeviceMessageSentEventArgs(string message)
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
        public ErrorMessageEventArgs(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }
    }
    public class DeviceTypeMessageEventArgs : EventArgs
    {
        public DeviceTypeMessageEventArgs(string deviceType)
        {
            DeviceType = deviceType;
        }

        public string DeviceType { get; }
    }
    public class LogMessageEventArgs : EventArgs
    {
        public LogMessageEventArgs(string log)
        {
            Log = log;
        }

        public string Log { get; }
    }
}
