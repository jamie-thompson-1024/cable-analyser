using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CableAnalyser
{
    public class Pin
    {
        PortPin _portPin;

        public Pin(PortType portType, int portNumber, int pin) {
            _portPin = new PortPin();
            _portPin.port = new Port();
            _portPin.port.portType = portType;
            _portPin.port.portNumber = portNumber;
            _portPin.pinNumber = pin;
        }
        public Pin(TesterConfiguration testerConfig, int pin) {
            _portPin = testerConfig.GetPortPinFromRawPin(pin);
        }

        public int GetRawPinNumber(TesterConfiguration portConfig)
        {
            return _pin;
        }

        public int PinNumber { get => _pin; }
        public int PortNumber { get => _portNumber; }
        public PortType PortType { get => _port; }

    }
}
