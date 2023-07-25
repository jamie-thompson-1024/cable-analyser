
using System;
using System.Diagnostics;
using System.Threading;

namespace DeviceConnector
{
    public class ArduinoConnector : IDeviceConnector
    {
        public event EventHandler<ErrorMessageEventArgs> ErrorMessage;
        public event EventHandler<LogMessageEventArgs> LogMessage;

        IDeviceConnection _connection;
        AutoResetEvent _autoResetEvent;
        int _timeout = 2000;

        string[] _responseArgs;

        public ArduinoConnector(IDeviceConnection connection)
        {
            _connection = connection;
            _connection.MessageReceived += MessageReceivedHandler;
            _autoResetEvent = new AutoResetEvent(false);
        }

        private void MessageReceivedHandler(object sender, DeviceMessageReceivedEventArgs e)
        {
            string rawMessage = e.Message;
            string[] commandArguments = rawMessage.Split(' ');

            _responseArgs = commandArguments;

            if (_responseArgs[0].Equals("Error"))
            {
                ErrorMessage?.Invoke(
                    this,
                    new ErrorMessageEventArgs(_responseArgs[1])
                );
            }

            if (_responseArgs[0].Equals("Log"))
            {
                LogMessage?.Invoke(
                    this,
                    new LogMessageEventArgs(_responseArgs[1])
                );
            }

            _autoResetEvent.Set();
        }

        private void SendMessageWait(string message)
        {
            _responseArgs = null;
            _connection.SendMessage(message);

            if (!_autoResetEvent.WaitOne(_timeout))
            {
                throw new TimeoutException("Timeout Waiting for Response from Arduino");
            }

            if (_responseArgs == null)
            {
                throw new TimeoutException("No / Empty Response from Arduino");
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
