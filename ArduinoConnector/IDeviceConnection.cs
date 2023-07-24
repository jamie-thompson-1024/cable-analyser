using System;
using System.Collections.Generic;

namespace DeviceConnector
{
    public enum MessageDirection
    {
        SEND, RECEIVE
    }

    public interface IDeviceConnection
    {
        event EventHandler<DeviceMessageSentEventArgs> MessageSent;
        event EventHandler<DeviceMessageReceivedEventArgs> MessageReceived;

        (MessageDirection, string)[] MessageHistory { get; }
        string ConnectedPort { get; }
        string[] AvaiablePorts { get; }
        void SendMessage(string message);
        void OpenConnection(string portName, int baudRate);
        void CloseConnection();
    }
}
