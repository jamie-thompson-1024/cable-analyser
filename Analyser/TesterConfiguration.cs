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
        PortType _portType;
        int _portNumber;
        int[] _pinConfig;
    }

    public class TesterConfiguration
    {
        List<Port> _ports;
        Dictionary<int, Port> _pinRelations;

        public TesterConfiguration(IEnumerable<Port> ports)
        {
            _ports = new List<Port>();
            _pinRelations = new Dictionary<int, Port>();

            // Create new instance of ports from given list
            // Index port instances by pins
            foreach (Port port in ports)
            {
                Port newPort = port.Clone();
                _ports.Add(newPort);
                foreach (int pin in newPort.PinConfig)
                {
                    _pinRelations.Add(pin, newPort);
                }
            }
        }

        public Port GetPortFromRawPin(int rawPin)
        {
            return _pinRelations[rawPin];
        }
    }
}
