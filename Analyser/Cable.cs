using DeviceConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CableAnalyser
{
    public class Cable
    {
        Connection[] _connections;

        public Cable() { 
        
        }

        public static Cable GenerateFromScan(IDeviceConnector connector)
        {

        }

        public (PortType, int)[] GetPorts()
        {
            List<(PortType, int)> ports = new List<(PortType, int)>();
            foreach(Connection connection in _connections)
            {

                (PortType, int) portA = (connection.PinA.PortType, connection.PinA.PortNumber);
                if (!ports.Contains(portA))
                {
                    ports.Add(portA);
                }

                (PortType, int) portB = (connection.PinB.PortType, connection.PinB.PortNumber);
                if (!ports.Contains((connection.PinB.PortType, connection.PinB.PortNumber)))
                {
                    ports.Add(portB);
                }
            }
        }
    }
}
