﻿using System;
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

        (MessageDirection, string)[] MessageHistory { get; }
        string ConnectedPort { get; }
        string[] AvaiablePorts { get; }
        void SendMessage(string message);
        void OpenConnection(string portName, int baudRate);
        void CloseConnection();
    }
}
