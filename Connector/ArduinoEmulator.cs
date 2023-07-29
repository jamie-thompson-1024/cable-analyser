using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceConnector
{
    public class ArduinoEmulator : IDeviceConnection
    {
        public (MessageDirection, string)[] MessageHistory => _messageHistory.ToArray();

        public string[] AvaiablePorts => _ports.ToArray();
        public string ConnectedPort => _connectedPort;

        public event EventHandler<DeviceMessageSentEventArgs> MessageSent;
        public event EventHandler<DeviceMessageReceivedEventArgs> MessageReceived;

        private List<(MessageDirection, string)> _messageHistory = new List<(MessageDirection, string)> ();
        private (int, int)[] _pinConnections;
        private int[] _testPins;
        private int[] _ioPins;
        private int _timeout;
        private string _connectedPort;
        private string[] _ports;

        public ArduinoEmulator((int, int)[] pinConnections, int[] ioPins, int[] testPins, int timeout, string[] ports)
        {
            _pinConnections = pinConnections;
            _ioPins = ioPins;
            _timeout = timeout;
            _ports = ports;
            _testPins = testPins;

            MessageReceived += (object sender, DeviceMessageReceivedEventArgs e) =>
            {
                _messageHistory.Add((MessageDirection.SEND, e.Message));
            };
        }

        public void CloseConnection()
        {
            _connectedPort = null;
        }

        public void OpenConnection(string portName, int baudRate)
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
            MessageReceived?.Invoke(this, new DeviceMessageReceivedEventArgs("DeviceType CableAnalyser"));
        }

        private void SetPinOutput(string[] arguments)
        {
            if (!_ioPins.Contains(int.Parse(arguments[1])))
            {
                SendError("INVALID_IO_PIN");
            }

            MessageReceived?.Invoke(
                this,
                new DeviceMessageReceivedEventArgs($"SetPinOutput Successful")
            );
        }

        private void TestPinConnections(string[] arguments)
        {
            int pin = int.Parse(arguments[1]);
            int[] testPins = Array.ConvertAll(
                arguments[2].Split(','), 
                new Converter<string, int>((pinStr) => int.Parse(pinStr))
            );

            if (!_testPins.Contains(pin))
            {
                SendError("INVALID_IO_PIN");
                return;
            }

            foreach (int testPin in testPins)
            {
                if(!_testPins.Contains(testPin))
                {
                    SendError("INVALID_TEST_PIN");
                    return;
                }
            };

            List<string> connectedPins = new List<string>();

            foreach (int testPin in testPins)
            {
                foreach ((int,int) pair in _pinConnections)
                {
                    if ((pair.Item1 == pin && pair.Item2 == testPin) || (pair.Item2 == pin && pair.Item1 == testPin))
                    {
                        connectedPins.Add(testPin.ToString());
                    }
                }
            }

            MessageReceived?.Invoke(
                this,
                new DeviceMessageReceivedEventArgs($"TestPinConnectionsResults {pin} {string.Join(",", connectedPins.ToArray())}")
            );
            
        }

        private void SendError(string message)
        {
            MessageReceived?.Invoke(this, new DeviceMessageReceivedEventArgs($"Error {message}"));
        }
    }
}
