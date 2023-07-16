namespace ArduinoConnector
{
    public class ArduinoConnector : IArduinoConnector
    {
        public event EventHandler<TestPinConnectionsMessageEventArgs> TestPinConnectionsMessage;
        public event EventHandler<TestPinConnectionsMessageEventArgs> DeviceTypeMessage;
        public event EventHandler<ErrorMessageEventArgs> ErrorMessage;

        IArduinoConnection _connection;

        public ArduinoConnector(IArduinoConnection connection)
        {
            _connection = connection;
            _connection.MessageReceived += MessageReceivedHandler;
        }

        private void MessageReceivedHandler(object sender, ArduinoMessageReceivedEventArgs e)
        {
            string rawMessage = e.Message;
            List<string> commandArguments = new List<string>(rawMessage.Split(' '));
            string command = commandArguments[0];
            commandArguments.RemoveAt(0);

            switch (command)
            {
                case "DeviceType":
                    OnDeviceTypeMessage(commandArguments.ToArray());
                    break;
                case "TestPinConnectionsResults":
                    OnTestPinConnectionsMessage(commandArguments.ToArray());
                    break;
                case "Error":
                    OnErrorMessage(commandArguments.ToArray());
                    break;
            }
        }

        private void OnDeviceTypeMessage(string[] arguments)
        {

        }

        private void OnErrorMessage(string[] arguments)
        {

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

            TestPinConnectionsMessage(
                this,
                new TestPinConnectionsMessageEventArgs(pin, testedPins)
            );
        }

        public void TestPinConnections(int pin, int[] testPins)
        {
            _connection.SendMessage($"TestPinConnections {pin} {String.Join(",", testPins)}");
        }

        public void SetPinOutput(int pin, bool state)
        {
            _connection.SendMessage($"SetPinOutput {pin} {(state ? "1" : "0")}");
        }

        public void GetDeviceType()
        {
            _connection.SendMessage("GetDeviceType");
        }
    }
}