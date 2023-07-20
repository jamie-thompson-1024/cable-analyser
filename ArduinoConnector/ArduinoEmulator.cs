﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArduinoConnector
{
    public class ArduinoEmulator : IArduinoConnection
    {
        public (MessageDirection, string)[] MessageHistory => _messageHistory.ToArray();

        public string[] AvaiablePorts => _ports.ToArray();
        public string ConnectedPort => _connectedPort;

        public event EventHandler<ArduinoMessageSentEventArgs> MessageSent;
        public event EventHandler<ArduinoMessageReceivedEventArgs> MessageReceived;

        private List<(MessageDirection, string)> _messageHistory = new List<(MessageDirection, string)> ();
        private (int, int)[] _pinConnections;
        private int[] _ioPins;
        private int _timeout;
        private string _connectedPort;
        private string[] _ports;

        public ArduinoEmulator((int, int)[] pinConnections, int[] ioPins, int timeout, string[] ports)
        {
            _pinConnections = pinConnections;
            _ioPins = ioPins;
            _timeout = timeout;
            _ports = ports;

            MessageReceived += (object sender, ArduinoMessageReceivedEventArgs e) =>
            {
                _messageHistory.Add((MessageDirection.SEND, e.Message));
            };
        }

        public void CloseConnection()
        {
            _connectedPort = null;
        }

        public void OpenConnection(string portName)
        {
            if (AvaiablePorts.Contains(portName))
            {
                _connectedPort = portName;
            } else
            {
                throw new Exception($"Port {portName} not available");
            }
        }

        public void SendMessage(string message)
        {
            if(_connectedPort == null) {
                throw new Exception("Failed to send message, no connection established");
            }

            _messageHistory.Add((MessageDirection.SEND, message));

            Task task = new Task(() => {
                Thread.Sleep(_timeout);

                string[] arguments = message.Split(' ');

                switch (arguments[0])
                {
                    case "GetDeviceType":
                        GetDeviceType(arguments);
                        break;
                    case "SetPinOutput":
                        SetPinOutput(arguments);
                        break;
                    case "TestPinConnections":
                        TestPinConnections(arguments);
                        break;
                }
            });
            task.Start();
        }

        private void GetDeviceType(string[] arguments)
        {
            MessageReceived(this, new ArduinoMessageReceivedEventArgs("DeviceType CableAnalyer"));
        }

        private void SetPinOutput(string[] arguments)
        {
            if (!_ioPins.Contains(int.Parse(arguments[1])))
            {
                SendError("INVALID_IO_PIN");
            }
        }

        private void TestPinConnections(string[] arguments)
        {
            
        }

        private void SendError(string message)
        {
            MessageReceived(this, new ArduinoMessageReceivedEventArgs($"Error {message}"));
        }
    }
}
