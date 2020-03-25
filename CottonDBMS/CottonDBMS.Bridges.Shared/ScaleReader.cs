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
    public class ScalePortReader : IDisposable
    {
        private SerialPort _serialPort = null;

        private string currentMessage = "";
                
        private int validWeightCount = 0;

        private int inMotionCount = 0;

        private string _messageLock = string.Empty;

        private int stableWeightSeconds = 5;

        private DateTime? firstStableWeight = null;

        private bool weightStable = false;

        public ScalePortReader(string serialPort)
        {
            _serialPort = new SerialPort(serialPort, 9600, Parity.None, 8, StopBits.One);
            //_serialPort.ReadBufferSize = 13;

            validWeightCount = 0;
            inMotionCount = 0;

            _serialPort.DataReceived += _serialPort_DataReceived;            
        }

        public DateTime? lastMessageTimeStamp = null;

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {                
                int dataLength = _serialPort.BytesToRead;
                byte[] data = new byte[dataLength];
                int nbrDataRead = _serialPort.Read(data, 0, dataLength);
                if (nbrDataRead == 0)
                    return;
                                
                string str = Encoding.ASCII.GetString(data);
                foreach(var ch in str.ToCharArray())
                {
                    if (ch != '\n' && ch != '\r')
                    {
                        currentMessage += ch;
                    }
                    else if (ch == '\r' || ch == '\n') {
                        if (currentMessage.Length > 0)
                        {
                            if (lastMessageTimeStamp.HasValue && lastMessageTimeStamp.Value.AddMilliseconds(300) > DateTime.Now) //was 250
                            {
                                currentMessage = string.Empty;
                                return;
                            }                                

                            var msg = new ScaleWeightReportMessage(currentMessage);
                            lastMessageTimeStamp = DateTime.Now;

                            Task.Run(() =>
                            {
                                lock (_messageLock)
                                {
                                    if (msg.Status == ScaleMessageStatus.VALID)
                                    {
                                        if (msg.Weight > 100.00M)
                                        {
                                            if (!firstStableWeight.HasValue)
                                            {
                                                firstStableWeight = DateTime.Now;
                                            }

                                            validWeightCount++;
                                            //if (validWeightCount > 20)
                                            if (firstStableWeight.HasValue && firstStableWeight.Value.AddSeconds(stableWeightSeconds) <= DateTime.Now && !weightStable)
                                            {
                                                //send weight acquired message
                                                weightStable = true;
                                                Messenger.Default.Send<WeightAcquiredMessage>(new WeightAcquiredMessage { Weight = msg.Weight });
                                            }
                                        }
                                    }
                                    else if (msg.Status == ScaleMessageStatus.MOTION)
                                    {
                                        weightStable = false;
                                        validWeightCount = 0;
                                        firstStableWeight = null; //clear first stable weight
                                        inMotionCount++;
                                        if (inMotionCount > 5) //was 20 - then 10
                                        {
                                            Messenger.Default.Send<InMotionMessage>(new InMotionMessage { Weight = msg.Weight });
                                        }
                                    }
                                    Messenger.Default.Send<ScaleWeightReportMessage>(msg);
                                }
                            });
                        }
                        currentMessage = string.Empty;
                    }
                }
               
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

        public void SetStableWeightSeconds(int seconds)
        {
            stableWeightSeconds = seconds;
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
