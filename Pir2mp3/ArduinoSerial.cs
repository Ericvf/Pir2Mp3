using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace Pir2mp3
{
    public class ArduinoSerial
    {
        public class DataEventArgs : EventArgs
        {
            public byte[] Bytes { get; set; }
        }

        public event EventHandler<DataEventArgs> DataReceived;

        protected SerialPort arduinoSerial;

        public bool IsOpen
        {
            get
            {
                if (arduinoSerial == null) return false;
                return arduinoSerial.IsOpen;
            }
        }

        public void Connect(int comPort, int baudRate = 9600)
        {
            this.Connect("COM" + comPort.ToString(), baudRate);
        }

        public void Connect(string comPort, int baudRate = 9600)
        {
            Console.WriteLine(string.Format("Establishing connection with Arduino device on {0} @ {1}... ", comPort, baudRate));

            arduinoSerial = new SerialPort();
            arduinoSerial.PortName = comPort;
            arduinoSerial.BaudRate = baudRate;
            arduinoSerial.DtrEnable = true;
            arduinoSerial.RtsEnable = true;
            arduinoSerial.DataReceived += new SerialDataReceivedEventHandler(arduinoSerial_DataReceived);

            try
            {
                // Open the serialPort
                arduinoSerial.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void arduinoSerial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var buffer = new List<byte>();
            while (arduinoSerial.BytesToRead > 0)
            {
                var receivedByte = (byte)arduinoSerial.ReadByte();
                buffer.Add(receivedByte);

                //Console.WriteLine($"Received byte: {receivedByte}");
            }

            DataReceived?.Invoke(this, new DataEventArgs()
            {
                Bytes = buffer.ToArray()
            });
        }

        public void Disconnect()
        {
            if (this.IsOpen)
            {
                Console.WriteLine("Disconnecting Arduino device");

                arduinoSerial.Close();
                arduinoSerial.Dispose();
            }
        }

        public virtual void Write(byte[] buffer)
        {
            if (arduinoSerial.IsOpen)
            {
                arduinoSerial.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
