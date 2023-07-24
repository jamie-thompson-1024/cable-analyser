using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConnector
{
    public class SerialConnection_Stream : IDeviceConnection
    {
        public (MessageDirection, string)[] MessageHistory => _messageHistory.ToArray();
        public string[] AvaiablePorts => SerialPort.GetPortNames();
        public string ConnectedPort => throw new NotImplementedException();

        public event EventHandler<DeviceMessageSentEventArgs> MessageSent;
        public event EventHandler<DeviceMessageReceivedEventArgs> MessageReceived;

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
        private void RaiseDeviceMessageSentEvent(DeviceMessageSentEventArgs e)
        {
            MessageSent?.Invoke(
                this, e
            );
        }
        private void RaiseDeviceMessageReceivedEvent(DeviceMessageReceivedEventArgs e)
        {
            MessageReceived?.Invoke(
                this, e
            );
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
            byte[] message = new byte[_maxReadBytes];
            while (!cancellationToken.IsCancellationRequested)
            { 
                int readBytes = await _serialPort.BaseStream.ReadAsync(buffer, 0, _maxReadBytes, cancellationToken);
                if (readBytes > 1)
                {
                    Array.Copy(buffer, 0, message, 1, readBytes);
                    string messageString = Encoding.ASCII.GetString(message, 0, readBytes + 1);
                    _messageHistory.Add((MessageDirection.RECEIVE, messageString));
                    RaiseDeviceMessageReceivedEvent(
                        new DeviceMessageReceivedEventArgs(messageString)
                    );
                }

                if (readBytes == 1)
                {
                    Array.Copy(buffer, 0, message, 0, readBytes);
                }
            }
        }

        public void SendMessage(string message)
        {
            _serialPort.Write(message);
            _messageHistory.Add((MessageDirection.SEND, message));

            RaiseDeviceMessageSentEvent(
                new DeviceMessageSentEventArgs(message)
            );
        }
        public void OpenConnection(string portName, int baudRate)
        {
            _serialPort.Close();
            _serialPort.Dispose();

            _serialPort.PortName = portName;
            _serialPort.BaudRate = baudRate;

            _serialPort.Parity = Parity.None;
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
