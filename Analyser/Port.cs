using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CableAnalyser
{
    public class Port
    {
        PortType _portType;
        int _portNumber;
        int[] _pinConfig;

        public Port(PortType portType, int portNumber, IEnumerable<int> pinConfig)
        {
            _portType = portType;
            _portNumber = portNumber;
            _pinConfig = pinConfig.ToArray();
        }
        public Port Clone()
        {
            return new Port(
                _portType, _portNumber, _pinConfig
            );
        }

        public IReadOnlyCollection<int> PinConfig { get => PinConfig; }
        public int PortNumber { get => _portNumber; }
        public PortType PortType { get => _portType; }
    }
}
