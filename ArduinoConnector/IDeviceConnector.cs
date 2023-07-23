using System;

namespace DeviceConnector
{
    public interface IDeviceConnector
    {
        event EventHandler<ErrorMessageEventArgs> ErrorMessage;
        event EventHandler<LogMessageEventArgs> LogMessage;

        string GetDeviceType();
        int[] TestPinConnections(int pin, int[] testPins);
        bool SetPinOutput(int pin, bool state);
    }
}
