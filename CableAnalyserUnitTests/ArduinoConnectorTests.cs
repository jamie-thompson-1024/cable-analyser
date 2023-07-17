using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArduinoConnector;

namespace CableAnalyserUnitTests
{
    [TestClass]
    public class ArduinoConnectorTests
    {
        [TestMethod]
        public void Request_TestPinConnections()
        {
        }

        [TestMethod]
        public void Request_SetPinOutput()
        {
        }

        [TestMethod]
        public void Request_GetDeviceType()
        {
            IArduinoConnection connection = new ArduinoEmulator();
            IArduinoConnector connector = new ArduinoConnector.ArduinoConnector(connection);

            Assert.AreEqual(
                "CableAnalyer",
                connector.GetDeviceType()
            );
        }
    }
}
