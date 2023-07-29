using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeviceConnector;
using System;
using System.Linq;

namespace CableAnalyserUnitTests
{
    [TestClass]
    public class ArduinoConnectorTests
    {
        static (int, int)[] pinConnections =
        {
            (5,9),
            (6,10),
            (7,11),
            (8,12),
            (8,10),
        };

        static int[] testPins =
        {
            5,6,7,8,9,10,11,12
        };

        static int[] ioPins =
        {
            13,14,15,16
        };

        static string[] serialPorts =
        {
            "COM1",
            "COM2"
        };

        static int messageTimeout = 500;

        static int baudRate = 57600;

        static bool useArduino = true;

        private IDeviceConnection ConnectionFactory()
        {
            IDeviceConnection connection;
            if (useArduino)
            {
                connection = new SerialConnection();
            }
            else
            {
                connection =  new ArduinoEmulator(
                    pinConnections, ioPins, testPins, messageTimeout, serialPorts
                );
            }

            if (connection.AvaiablePorts.Length == 0)
            {
                throw new Exception("No Avaialble Ports");
            }

            return connection;
        }
        private IDeviceConnector ConnectorFactory(IDeviceConnection connection)
        {
            return new ArduinoConnector(connection);
        }

        [TestMethod]
        public void Request_TestPinConnections()
        {
            IDeviceConnection connection = ConnectionFactory();
            IDeviceConnector connector = ConnectorFactory(connection);
            connection.OpenConnection(connection.AvaiablePorts[0], baudRate);

            int pin = 11;
            int[] testPins = { 7, 10, 6, 9 };

            int[] expected = { 7 };

            Assert.IsTrue(Enumerable.SequenceEqual(
                expected,
                connector.TestPinConnections(pin, testPins)
            ));

            connection.CloseConnection();
        }

        [TestMethod]
        public void Request_SetPinOutput()
        {
            IDeviceConnection connection = ConnectionFactory();
            IDeviceConnector connector = ConnectorFactory(connection);
            connection.OpenConnection(connection.AvaiablePorts[0], baudRate);

            Assert.IsTrue(
                connector.SetPinOutput(ioPins[0], true)
            );

            connection.CloseConnection();
        }

        [TestMethod]
        public void Request_GetDeviceType()
        {
            IDeviceConnection connection = ConnectionFactory();
            IDeviceConnector connector = ConnectorFactory(connection);
            connection.OpenConnection(connection.AvaiablePorts[0], baudRate);

            string deviceType = connector.GetDeviceType();

            Assert.IsTrue(
                deviceType.Equals("CableAnalyser")
            );

            connection.CloseConnection();
        }
    }
}
