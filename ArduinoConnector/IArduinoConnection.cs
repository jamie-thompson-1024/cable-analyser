using System;
using System.Collections.Generic;

namespace ArduinoConnector
{
    public enum MessageDirection
    {
        SEND, RECEIVE
    }

    public interface IArduinoConnection
    {
        event EventHandler<ArduinoMessageSentEventArgs> MessageSent;
        event EventHandler<ArduinoMessageReceivedEventArgs> MessageReceived;

        List<(MessageDirection, string)> MessageHistory{ get; }
        string[] AvaiablePorts { get; }
        void SendMessage(string message);
        void OpenConnection();
        void CloseConnection();
    }
}
