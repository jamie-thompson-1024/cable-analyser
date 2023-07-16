using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConnector
{
    public interface IArduinoConnector
    {
        event EventHandler<TestPinConnectionsMessageEventArgs> TestPinConnectionsMessage;
        event EventHandler<DeviceTypeMessageEventArgs> DeviceTypeMessage;
        event EventHandler<ErrorMessageEventArgs> ErrorMessage;

        void GetDeviceType();
        void TestPinConnections(int pin, int[] testPins);
        void SetPinOutput(int pin, bool state);
    }
}
