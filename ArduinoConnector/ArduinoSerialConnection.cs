using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConnector
{
    public class ArduinoSerialConnection : IArduinoConnection
    {
        public event EventHandler<ArduinoMessageSentEventArgs> MessageSent;
        public event EventHandler<ArduinoMessageReceivedEventArgs> MessageReceived;

        SerialPort _serialPort;
        List<(string,string)> _messageHistory;

        public ArduinoSerialConnection(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.DataReceived += ReceivedMessageHandler;
        }

        public List<(string,string)> MessageHistory
        {
            get
            {
                return _messageHistory.ConvertAll(
                    new Converter<(string, string), (string, string)>(x => x)
                );
            }
        }

        public string[] AvaiablePorts => SerialPort.GetPortNames();

        public void SendMessage(string message)
        {
            _serialPort.WriteLine(message);
            _messageHistory.Add(("Sent", message));
            MessageSent(
                this, 
                new ArduinoMessageSentEventArgs(message)
            );
        }
        private void ReceivedMessageHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort port = (SerialPort)sender;
            string message = port.ReadLine();
            _messageHistory.Add(("Received", message));
            MessageReceived(
                this,
                new ArduinoMessageReceivedEventArgs(message)
            );
        }
        public void OpenConnection()
        {
            _serialPort.Open();
        }
        public void CloseConnection()
        {
            _serialPort.Close();
        }
    }
}
