﻿using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace ArduinoConnector
{
    public class ArduinoSerialConnection : IArduinoConnection
    {
        public (MessageDirection, string)[] MessageHistory => _messageHistory.ToArray();

        public event EventHandler<ArduinoMessageSentEventArgs> MessageSent;
        public event EventHandler<ArduinoMessageReceivedEventArgs> MessageReceived;

        SerialPort _serialPort;
        List<(MessageDirection, string)> _messageHistory;

        public ArduinoSerialConnection(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.DataReceived += ReceivedMessageHandler;
        }

        public string[] AvaiablePorts => SerialPort.GetPortNames();

        public void SendMessage(string message)
        {
            _serialPort.WriteLine(message);
            _messageHistory.Add((MessageDirection.SEND, message));
            MessageSent(
                this, 
                new ArduinoMessageSentEventArgs(message)
            );
        }
        private void ReceivedMessageHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort port = (SerialPort)sender;
            string message = port.ReadLine();
            _messageHistory.Add((MessageDirection.RECEIVE, message));
            MessageReceived(
                this,
                new ArduinoMessageReceivedEventArgs(message)
            );
        }
        public void OpenConnection(string portName)
        {
            _serialPort.Open();
        }
        public void CloseConnection()
        {
            _serialPort.Close();
        }
    }
}
