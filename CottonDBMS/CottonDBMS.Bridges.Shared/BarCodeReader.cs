using CottonDBMS.Bridges.Shared.Messages;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.Bridges.Shared
{
    public class BarcodeReader : IDisposable
    {
        private SerialPort _serialPort = null;
        
        public BarcodeReader(string serialPort)
        {
            _serialPort = new SerialPort(serialPort, 9600, Parity.None, 8, StopBits.One);
            _serialPort.DataReceived += _serialPort_DataReceived;
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int dataLength = _serialPort.BytesToRead;
                byte[] data = new byte[dataLength];
                int nbrDataRead = _serialPort.Read(data, 0, dataLength);
                if (nbrDataRead == 0)
                    return;
                Messenger.Default.Send<BarcodeScannedMessage>(new BarcodeScannedMessage { Data = Encoding.ASCII.GetString(data)});                
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        public void Start()
        {
            try
            {
                if (_serialPort != null && !_serialPort.IsOpen)
                {
                    _serialPort.Open();
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        public bool IsOpen
        {
            get
            {
                return (_serialPort != null && _serialPort.IsOpen);
            }
        }

        public void Stop()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        public void Dispose()
        {
            _serialPort.DataReceived -= _serialPort_DataReceived;
            if (_serialPort != null)
            {
                if (_serialPort.IsOpen)
                    _serialPort.Close();
                _serialPort.Dispose();
            }
        }
    }
}
