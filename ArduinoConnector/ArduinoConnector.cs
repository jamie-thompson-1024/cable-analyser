
using System;
using System.Diagnostics;
using System.Threading;

namespace ArduinoConnector
{
    public class ArduinoConnector : IArduinoConnector
    {
        public event EventHandler<ErrorMessageEventArgs> ErrorMessage;
        public event EventHandler<LogMessageEventArgs> LogMessage;

        IArduinoConnection _connection;
        AutoResetEvent _autoResetEvent;
        int _timeout = 2000;

        string[] _responseArgs;

        public ArduinoConnector(IArduinoConnection connection)
        {
            _connection = connection;
            _connection.MessageReceived += MessageReceivedHandler;
            _autoResetEvent = new AutoResetEvent(false);
        }

        private void MessageReceivedHandler(object sender, ArduinoMessageReceivedEventArgs e)
        {
            Debug.Print("Messgae Rx");

            string rawMessage = e.Message;
            string[] commandArguments = rawMessage.Split(' ');

            _responseArgs = commandArguments;

            if (_responseArgs[0].Equals("Error"))
            {
                ErrorMessage(
                    this,
                    new ErrorMessageEventArgs(_responseArgs[1])
                );
            }

            Debug.Print("Messgae Rx");

            _autoResetEvent.Set();
        }

        private void SendMessageWait(string message)
        {
            _responseArgs = null;
            _connection.SendMessage(message);
            _autoResetEvent.WaitOne(_timeout);

            if (_responseArgs == null)
            {
                throw new TimeoutException("Timeout Waiting for Response from Arduino");
            }

            if (_responseArgs[0].Equals("Error"))
            {
                throw new Exception($"Error {_responseArgs[1]}");
            }
        }

        public int[] TestPinConnections(int pin, int[] testPins)
        {
            SendMessageWait($"TestPinConnections {pin} {String.Join(",", testPins)}");

            if (!_responseArgs[0].Equals("TestPinConnectionsResults"))
            {
                throw new Exception($"Unexpected Response, Expected 'TestPinConnectionsResults' Got '{_responseArgs[0]}'");
            }

            if (int.Parse(_responseArgs[1]) != pin)
            {
                throw new Exception("Unexpected Pin Number on Response");
            }

            int[] testedPins = { };

            if (!_responseArgs[2].Equals("N/C"))
            {
                testedPins = Array.ConvertAll(
                    _responseArgs[2].Split(','),
                    new Converter<string, int>(x => int.Parse(x))
                );
            }

            return testedPins;
        }

        public bool SetPinOutput(int pin, bool state)
        {
            SendMessageWait($"SetPinOutput {pin} {(state ? "1" : "0")}");

            if (!_responseArgs[0].Equals("SetPinOutput"))
            {
                throw new Exception($"Unexpected Response, Expected 'SetPinOutput' Got '{_responseArgs[0]}'");
            }

            return true;
        }

        public string GetDeviceType()
        {
            SendMessageWait("GetDeviceType");

            if (!_responseArgs[0].Equals("DeviceType"))
            {
                throw new Exception($"Unexpected Response, Expected 'DeviceType' Got '{_responseArgs[0]}'");
            }

            string deviceType = _responseArgs[1];

            return deviceType;
        }
    }
}