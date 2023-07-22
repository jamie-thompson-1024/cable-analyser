using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArduinoConnector;
using System;
using System.Linq;
using System.Collections.ObjectModel;

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
            1,2,3,4
        };

        static string[] serialPorts =
        {
            "COM1",
            "COM2"
        };

        static int messageTimeout = 250;

        static bool useArduino = false;

        private IArduinoConnection ConnectionFactory()
        {
            if (useArduino)
            {
                return new ArduinoSerialConnection();
            }

            return new ArduinoEmulator(
                pinConnections, ioPins, testPins, messageTimeout, serialPorts
            );
        }
        private IArduinoConnector ConnectorFactory(IArduinoConnection connection)
        {
            return new ArduinoConnector.ArduinoConnector(connection);
        }

        [TestMethod]
        public void Request_TestPinConnections()
        {
            IArduinoConnection connection = ConnectionFactory();
            IArduinoConnector connector = ConnectorFactory(connection);
            connection.OpenConnection(connection.AvaiablePorts[0], 9600);

            int pin = 11;
            int[] testPins = { 7, 10, 6, 9 };

            int[] expected = { 7 };

            Assert.IsTrue(Enumerable.SequenceEqual(
                expected,
                connector.TestPinConnections(pin, testPins)
            ));
        }

        [TestMethod]
        public void Request_SetPinOutput()
        {
            IArduinoConnection connection = ConnectionFactory();
            IArduinoConnector connector = ConnectorFactory(connection);
            connection.OpenConnection(connection.AvaiablePorts[0], 9600);

        }

        [TestMethod]
        public void Request_GetDeviceType()
        {
            IArduinoConnection connection = ConnectionFactory();
            IArduinoConnector connector = ConnectorFactory(connection);
            connection.OpenConnection(connection.AvaiablePorts[0], 9600);

            Assert.AreEqual(
                "CableAnalyer",
                connector.GetDeviceType()
            );
        }
    }
}
