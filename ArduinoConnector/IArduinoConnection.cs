using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConnector
{
    public interface IArduinoConnection
    {
        event EventHandler<ArduinoMessageSentEventArgs> MessageSent;
        event EventHandler<ArduinoMessageReceivedEventArgs> MessageReceived;

        List<(string, string)> MessageHistory{ get; }
        string[] AvaiablePorts { get; }
        void SendMessage(string message);
        void OpenConnection();
        void CloseConnection();
    }
}
