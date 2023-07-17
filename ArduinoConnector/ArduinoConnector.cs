
using System;
using System.Threading;

namespace ArduinoConnector
{
    public class ArduinoConnector : IArduinoConnector
    {
        //public event EventHandler<TestPinConnectionsMessageEventArgs> TestPinConnectionsMessage;
        //public event EventHandler<DeviceTypeMessageEventArgs> DeviceTypeMessage;
        public event EventHandler<ErrorMessageEventArgs> ErrorMessage;
        public event EventHandler<LogMessageEventArgs> LogMessage;

        IArduinoConnection _connection;
        AutoResetEvent _autoResetEvent;

        string[] _responseArgs;

        public ArduinoConnector(IArduinoConnection connection)
        {
            _connection = connection;
            _connection.MessageReceived += MessageReceivedHandler;
            _autoResetEvent = new AutoResetEvent(false);
        }

        private void MessageReceivedHandler(object sender, ArduinoMessageReceivedEventArgs e)
        {
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

            _autoResetEvent.Set();

            /*
            switch (commandArguments[0])
            {
                case "DeviceType":
                    //OnDeviceTypeMessage(commandArguments.ToArray());
                    break;
                case "TestPinConnectionsResults":
                    //OnTestPinConnectionsMessage(commandArguments.ToArray());
                    break;
                case "Error":
                    //OnErrorMessage(commandArguments.ToArray());
                    break;
            }
            */
        }

        /*
        private void OnDeviceTypeMessage(string[] arguments)
        {
            string deviceType = arguments[0];
            //DeviceTypeMessage(this, new DeviceTypeMessageEventArgs(deviceType));
        }

        private void OnErrorMessage(string[] arguments)
        {
            ArduinoErrorCode errorCode = (ArduinoErrorCode)int.Parse(arguments[0]);
            ErrorMessage(this, new ErrorMessageEventArgs(errorCode));
        }

        private void OnTestPinConnectionsMessage(string[] arguments)
        {
            int pin = int.Parse(arguments[0]);
            int[] testedPins = {};

            if (!arguments[1].Equals("N/C"))
            {
                testedPins = Array.ConvertAll<string, int>(
                    arguments[1].Split(','),
                    new Converter<string, int>(x => int.Parse(x))
                );
            }

            //TestPinConnectionsMessage(
            //    this,
            //    new TestPinConnectionsMessageEventArgs(pin, testedPins)
            //);
        }
        */

        public int[] TestPinConnections(int pin, int[] testPins)
        {
            _connection.SendMessage($"TestPinConnections {pin} {String.Join(",", testPins)}");
            _autoResetEvent.WaitOne(500);

            if (_responseArgs == null)
            {
                throw new TimeoutException("Timedout Waiting for Response from Arduino");
            }

            if (_responseArgs[0].Equals("Error"))
            {
                throw new Exception($"Error {_responseArgs[1]}");
            }

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
                testedPins = Array.ConvertAll<string, int>(
                    _responseArgs[2].Split(','),
                    new Converter<string, int>(x => int.Parse(x))
                );
            }

            _responseArgs = null;
            return testedPins;
        }

        public void SetPinOutput(int pin, bool state)
        {
            _connection.SendMessage($"SetPinOutput {pin} {(state ? "1" : "0")}");
        }

        public string GetDeviceType()
        {
            _connection.SendMessage("GetDeviceType");
            _autoResetEvent.WaitOne(500);

            if (_responseArgs == null)
            {
                throw new TimeoutException("Timedout Waiting for Response from Arduino");
            }

            if (_responseArgs[0].Equals("Error"))
            {
                throw new Exception($"Error {_responseArgs[1]}");
            }

            if (!_responseArgs[0].Equals("DeviceType"))
            {
                throw new Exception($"Unexpected Response, Expected 'DeviceType' Got '{_responseArgs[0]}'");
            }

            string deviceType = _responseArgs[1];
            _responseArgs = null;

            return deviceType;
        }

        public void OpenConnection()
        {
            _connection.OpenConnection();
        }

        public void CloseConnection()
        {
            _connection.CloseConnection();
        }
    }
}