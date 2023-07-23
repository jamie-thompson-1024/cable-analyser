using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConnector
{
    public class SerialConnection_Stream : IDeviceConnection
    {
        public (MessageDirection, string)[] MessageHistory => _messageHistory.ToArray();
        public string[] AvaiablePorts => SerialPort.GetPortNames();
        public string ConnectedPort => throw new NotImplementedException();

        public event EventHandler<ArduinoMessageSentEventArgs> MessageSent;
        public event EventHandler<ArduinoMessageReceivedEventArgs> MessageReceived;

        SerialPort _serialPort;
        List<(MessageDirection, string)> _messageHistory;

        Task _readTask;
        CancellationTokenSource _readTaskCancellationTokenSource;
        int _maxReadBytes = 1024;

        public SerialConnection_Stream()
        {
            _serialPort = new SerialPort();
            _serialPort.Dispose();

            _messageHistory = new List<(MessageDirection, string)>();
        }

        private void BeginRead()
        {
            _readTaskCancellationTokenSource?.Dispose();
            _readTaskCancellationTokenSource = new CancellationTokenSource();

            CancellationToken cancellationToken = _readTaskCancellationTokenSource.Token;

            _readTask = Task.Run(
                () => { ReadLoop(cancellationToken); },
                cancellationToken
            );
        }

        private void EndRead()
        {
            _readTaskCancellationTokenSource.Cancel();
            _readTask.Wait();
        }

        private async void ReadLoop(CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[_maxReadBytes];
            while (!cancellationToken.IsCancellationRequested)
            { 
                int readBytes = await _serialPort.BaseStream.ReadAsync(buffer, 0, _maxReadBytes, cancellationToken);
                string message = Encoding.ASCII.GetString(buffer, 0, readBytes);
                Debug.Print(readBytes.ToString());
                Debug.Print(message);
                if (readBytes > 0)
                {
                    _messageHistory.Add((MessageDirection.RECEIVE, message));
                    MessageReceived?.Invoke(
                        this,
                        new ArduinoMessageReceivedEventArgs(message)
                    );
                }
            }
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
            BeginRead();
        }
        public void CloseConnection()
        {
            EndRead();
            _serialPort.Close();
            _serialPort.Dispose();
        }
    }
}
