using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CableAnalyser
{
    public struct PortConfiguration
    {
        public Port port;
        public int[] pinConfig;
    }
    public struct PortPin
    {
        public Port port;
        public int pinNumber;
    }

    public class TesterConfiguration
    {
        List<PortConfiguration> _ports;
        Dictionary<int, PortConfiguration> _pinRelations;

        public TesterConfiguration(IEnumerable<PortConfiguration> ports)
        {
            _ports = new List<PortConfiguration>();
            _pinRelations = new Dictionary<int, PortConfiguration>();

            // Create new instance of ports from given list
            // Index port instances by pins
            foreach (PortConfiguration port in ports)
            {
                _ports.Add(port);
                foreach (int pin in port.pinConfig)
                {
                    _pinRelations.Add(pin, port);
                }
            }
        }

        public PortPin GetPortPinFromRawPin(int rawPin)
        {
            PortConfiguration portConfig = _pinRelations[rawPin];
            int[] pinNumbers = portConfig.pinConfig;
            for (int i = 0; i < pinNumbers.Length; i++)
            {
                if (pinNumbers[i] == rawPin)
                {
                    PortPin pin = new PortPin();
                    pin.port = portConfig.port;
                    pin.pinNumber = i;
                    return pin;
                }
            }
            throw new Exception($"rawPin {rawPin} is invalid");
        }

        public int GetRawPinFromPortPin(PortPin portPin)
        {
            foreach(PortConfiguration portConfig in _ports)
            {
                if(portConfig.port == portPin.port) {
                    return portConfig.pinConfig[portPin.pinNumber];
                }
            }
            throw new Exception($"portPin {PortTypeMethods.ToString(portPin.port.portType)} {portPin.port.portNumber}: {portPin.pinNumber} is invalid");
        }
    }
}
