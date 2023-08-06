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

        public (PortType, int)[] GetPorts()
        {
            List<(PortType, int)> ports = new List<(PortType, int)>();
            foreach(Connection connection in _connections)
            {
                (PortType, int) port = 

                if (!ports.Contains((connection.PinA.PortType, connection.PinA.PortNumber)))
                {
                    ports.Add()
                }

                if (!ports.Contains((connection.PinB.PortType, connection.PinB.PortNumber)))
                {

                }
            }
        }
    }
}
