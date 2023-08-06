using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CableAnalyser
{
    public class Pin
    {
        PortType _port;
        int _portNumber;
        int _pin;

        public Pin(PortType port, int portNumber, int pin) {
            _port = port;
            _pin = pin;
            _portNumber = portNumber;
        }
        public Pin(TesterConfiguration portConfig, int pin) { 
        
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
