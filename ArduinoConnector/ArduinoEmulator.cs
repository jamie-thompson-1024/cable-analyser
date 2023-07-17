using System;
using System.Collections.Generic;

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
            string[] arguments = message.Split(' ');

            switch (arguments[0])
            {
                case "GetDeviceType":
                    MessageReceived(this, new ArduinoMessageReceivedEventArgs("DeviceType CableAnalyer"));
                    break;
            }
        }
    }
}
