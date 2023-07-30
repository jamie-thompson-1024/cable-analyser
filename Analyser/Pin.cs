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
        int _pin;

        public Pin(PortType port, int pin) {
            _port = port;
            _pin = pin;
        }
        public Pin(PortConfiguration portConfig, int pin) { 
        
        }

        public int GetRawPinNumber(PortConfiguration portConfig)
        {
            return _pin;
        }

        public int PinNumber { get => _pin; }
        public PortType PortType { get => _port; }

    }
}
