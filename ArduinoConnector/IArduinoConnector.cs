using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConnector
{
    public interface IArduinoConnector
    {
        //event EventHandler<TestPinConnectionsMessageEventArgs> TestPinConnectionsMessage;
        //event EventHandler<DeviceTypeMessageEventArgs> DeviceTypeMessage;
        event EventHandler<ErrorMessageEventArgs> ErrorMessage;
        event EventHandler<LogMessageEventArgs> LogMessage;

        void OpenConnection();
        void CloseConnection();
        string GetDeviceType();
        int[] TestPinConnections(int pin, int[] testPins);
        void SetPinOutput(int pin, bool state);
    }
}
