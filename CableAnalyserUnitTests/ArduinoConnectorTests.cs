using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArduinoConnector;
using System;

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
        };

        static int[] ioPins =
        {
            1,2,3,4
        };

        [TestMethod]
        public void Request_TestPinConnections()
        {
            IArduinoConnection connection = new ArduinoEmulator(pinConnections, ioPins, 250);
            IArduinoConnector connector = new ArduinoConnector.ArduinoConnector(connection);
            connection.OpenConnection();

        }

        [TestMethod]
        public void Request_SetPinOutput()
        {
            IArduinoConnection connection = new ArduinoEmulator(pinConnections, ioPins, 250);
            IArduinoConnector connector = new ArduinoConnector.ArduinoConnector(connection);
            connection.OpenConnection();

        }

        [TestMethod]
        public void Request_GetDeviceType()
        {
            IArduinoConnection connection = new ArduinoEmulator(pinConnections, ioPins, 250);
            IArduinoConnector connector = new ArduinoConnector.ArduinoConnector(connection);
            connection.OpenConnection();

            Assert.AreEqual(
                "CableAnalyer",
                connector.GetDeviceType()
            );
        }

        [TestMethod]
        public void Request_GetDeviceType_Timeout()
        {
            IArduinoConnection connection = new ArduinoEmulator(1000);
            IArduinoConnector connector = new ArduinoConnector.ArduinoConnector(connection);
            connection.OpenConnection();

            Assert.ThrowsException<TimeoutException>(() => connector.GetDeviceType() );
        }
    }
}
