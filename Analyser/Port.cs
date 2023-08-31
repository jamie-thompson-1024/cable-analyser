using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CableAnalyser
{
    public struct Port
    {
        public PortType portType;
        public int portNumber;

        public static bool operator ==(Port left, Port right)
        {
            return (left.portType == right.portType && left.portNumber == right.portNumber);
        }
        public static bool operator !=(Port left, Port right)
        {
            return (left.portType != right.portType || left.portNumber != right.portNumber);
        }
    }
}
