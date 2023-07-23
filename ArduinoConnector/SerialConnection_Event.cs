using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;

namespace DeviceConnector
{
    public class SerialConnection_Event : IDeviceConnection
    {
        public (MessageDirection, string)[] MessageHistory => _messageHistory.ToArray();
        public string[] AvaiablePorts => SerialPort.GetPortNames();
        public string ConnectedPort => throw new NotImplementedException();

        public event EventHandler<ArduinoMessageSentEventArgs> MessageSent;
        public event EventHandler<ArduinoMessageReceivedEventArgs> MessageReceived;

        SerialPort _serialPort;
        List<(MessageDirection, string)> _messageHistory;

        public SerialConnection_Event()
        {
            _serialPort = new SerialPort();
            _serialPort.DataReceived += ReceivedMessageHandler;
            _serialPort.Dispose();

            _messageHistory = new List<(MessageDirection, string)>();
        }

        public void SendMessage(string message)
        {
            _serialPort.Write(message);
            _messageHistory.Add((MessageDirection.SEND, message));

            MessageSent?.Invoke(
                this, 
                new ArduinoMessageSentEventArgs(message)
            );
        }
        private void ReceivedMessageHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort port = (SerialPort)sender;
            string message = port.ReadLine();
            _messageHistory.Add((MessageDirection.RECEIVE, message));

            MessageReceived?.Invoke(
                this,
                new ArduinoMessageReceivedEventArgs(message)
            );
        }
        public void OpenConnection(string portName, int baudRate)
        {
            _serialPort.Close();
            _serialPort.Dispose();

            _serialPort.PortName = portName;
            _serialPort.BaudRate = baudRate;

            _serialPort.Parity = Parity.Even;
            _serialPort.StopBits = StopBits.One;
            _serialPort.DataBits = 8;

            _serialPort.Handshake = Handshake.None;
            _serialPort.DtrEnable = false;
            _serialPort.RtsEnable = false;

            _serialPort.Open();
        }
        public void CloseConnection()
        {
            _serialPort.Close();
            _serialPort.Dispose();
        }
    }
}
