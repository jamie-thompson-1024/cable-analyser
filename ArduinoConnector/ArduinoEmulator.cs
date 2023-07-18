using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ArduinoConnector
{
    public class ArduinoEmulator : IArduinoConnection
    {
        public List<(MessageDirection, string)> MessageHistory => throw new NotImplementedException();

        public string[] AvaiablePorts => throw new NotImplementedException();

        public event EventHandler<ArduinoMessageSentEventArgs> MessageSent;
        public event EventHandler<ArduinoMessageReceivedEventArgs> MessageReceived;

        private (int, int)[] _pinConnections;
        private int[] _ioPins;

        public ArduinoEmulator()
        {}

        public ArduinoEmulator((int, int)[] pinConnections, int[] ioPins)
        {
            _pinConnections = pinConnections;
            _ioPins = ioPins;
        }

        public void CloseConnection()
        {
            throw new NotImplementedException();
        }

        public void OpenConnection()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(string message)
        {
            Task task = new Task(() => {
                Thread.Sleep(250);

                string[] arguments = message.Split(' ');

                switch (arguments[0])
                {
                    case "GetDeviceType":
                        MessageReceived(this, new ArduinoMessageReceivedEventArgs("DeviceType CableAnalyer"));
                        break;
                }
            });
            task.Start();
        }
    }
}
