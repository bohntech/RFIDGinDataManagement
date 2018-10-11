//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USDigital;
using GalaSoft.MvvmLight.Messaging;
using CottonDBMS.TruckApp.Enums;
using CottonDBMS.TruckApp.Messages;
using GalaSoft.MvvmLight.Ioc;
using CottonDBMS.Interfaces;
using CottonDBMS.DataModels;

namespace CottonDBMS.TruckApp.DataProviders
{
    

    public static class QuadratureEncoderDataProvider
    {
        /// <summary>
        /// ENCODER_PRESET represents number of steps per rotation
        /// </summary>
        private const uint ENCODER_PRESET = 1800 - 1;

        /// <summary>
        /// Default streaming interval represents how often updates are read from the encoder
        /// </summary>
        private const ushort DEFAULT_STREAMING_INTERVAL = 65;       

        /// <summary>
        /// Represents how much the count must change before an updated value is received
        /// </summary>
        private const uint ENCODER_COUNT_THRESHOLD = 5;
        private const uint OUTPUT_INTERVAL_RATE = 60;        
        private const uint STOP_CHANGE_THRESHOLD = 5;
        private const uint MIN_RPM_THRESHOLD = 1;

        private static DeviceManager mDeviceManager;
        private static QSB_S mQSB;

        private static uint lastCount = 0;
        private static uint lastCountTimeStamp = 0;
        private static double lastVelocity = 0;

        private static Queue<QuadratureStateChangeMessage> eventQueue = new Queue<QuadratureStateChangeMessage>();        
        private static DirectionOfRotation DirectionOfRotation = DirectionOfRotation.Stopped;

        private static System.Timers.Timer pollTimer = null;
        
        private static object locker = new object();
        private static int stopCount = 0;        

        static QuadratureEncoderDataProvider()
        {
            try
            {
                mDeviceManager = new DeviceManager();
                mDeviceManager.Initialize();
                mQSB = (QSB_S)mDeviceManager.GetDeviceOfType(0, typeof(QSB_S));
                pollTimer = new System.Timers.Timer(100);
                pollTimer.Enabled = false;
                pollTimer.AutoReset = false;                
                pollTimer.Elapsed += PollTimer_Elapsed;                
            }
            catch(Exception exc)
            {
                Logging.Logger.Log(exc);                
            }
        }

        private static void PollTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (locker)
            {
                try
                {
                    var thisCount = mQSB.GetCount();
                    uint countDelta = 0;
                    bool positiveChange = false;
                    if (thisCount > lastCount)
                    {
                        positiveChange = true;
                        countDelta = thisCount - lastCount;
                    }
                    else
                    {
                        countDelta = lastCount - thisCount;
                    }
                    lastCount = thisCount;
                    DirectionOfRotation oldDirection = DirectionOfRotation;
                    if (countDelta < 40)
                    {
                        //not changed since last interval
                        if (stopCount < 4)  //must read a stopped value 4 times (for 400ms) to trigger a stop event
                        {
                            stopCount++;
                        }
                        else
                        {
                            DirectionOfRotation = DirectionOfRotation.Stopped;
                            stopCount = 0;
                        }
                    }
                    else if (countDelta >= 40 && positiveChange)
                    {
                        DirectionOfRotation = DirectionOfRotation.RotatingClockwise;
                    }
                    else if (countDelta >= 40)
                    {
                        DirectionOfRotation = DirectionOfRotation.RotatingCounterClockwise;
                    }
                    if (oldDirection != DirectionOfRotation)
                    {
                        if (oldDirection != DirectionOfRotation.Stopped && DirectionOfRotation != DirectionOfRotation.Stopped)
                        {
                            FireStopStateChange();
                        }

                        FireStateChange();
                    }
                }
                catch (Exception exc)
                {
                    Logging.Logger.Log(exc);
                }
            }
            pollTimer.Start();          
        }

        public static void StartEvents()
        {
            ConfigQsb();
                        
            if (mQSB != null)
            {
                pollTimer.Start();
                //FireStateChange();
            }
           
        }   
        
        public static bool IsStarted
        {
            get
            {
                return mQSB != null;
            }
        }            

        private static void ConfigQsb()
        {
            if (mQSB != null)
            {
                mQSB.SetCounterMode(CounterMode.FreeRunningCounter);
                mQSB.SetQuadratureMode(QuadratureMode.X1);
                mQSB.SetPreset(ENCODER_PRESET);

                lastCount = Convert.ToUInt32(uint.MaxValue / 2);
                mQSB.SetCount(lastCount);       
            }
        }

        /*private static void MQSB_OnRegisterValueChanged(object sender, RegisterChangeEventArgs args)
        {
            DirectionOfRotation oldDirection = DirectionOfRotation;

            if (args.Register == (byte)QSB.Register.ReadEncoder)
            {
                double timeDiffBetweenReadsInMinutes = Math.Abs(args.TimeStamp - lastCountTimeStamp) *
                                                       QSB.TIMESTAMP_INTERVAL_IN_MINUTES;
                lastCountTimeStamp = args.TimeStamp;

                double countDelta = Math.Abs(args.Value - (long)lastCount);
                
                bool pos = false;

                pos = (args.Value > lastCount);
                lastCount = args.Value;

                if (DirectionOfRotation == DirectionOfRotation.Stopped && countDelta < STOP_CHANGE_THRESHOLD)
                {
                    DirectionOfRotation = DirectionOfRotation.Stopped;
                }
                else
                {
                    uint countsPerRevolution = ENCODER_PRESET + 1;
                    if (timeDiffBetweenReadsInMinutes > 0)
                    {
                        //VELOCITY IS VELOCITY IN RPMs
                        var velocity = (countDelta / countsPerRevolution) / timeDiffBetweenReadsInMinutes;
                        double acceleration = Math.Abs(velocity - lastVelocity) / timeDiffBetweenReadsInMinutes;
                        lastVelocity = velocity;

                        //ONLY CONSIDER THE SHAFT TO BE MOVING IF THE VELOCITY IS ABOVE CERTAIN NUMBER OF RPMS
                        if (pos && velocity > MIN_RPM_THRESHOLD)
                            DirectionOfRotation = DirectionOfRotation.RotatingClockwise;
                        else if (velocity > MIN_RPM_THRESHOLD)
                            DirectionOfRotation = DirectionOfRotation.RotatingCounterClockwise;
                        else
                            DirectionOfRotation = DirectionOfRotation.Stopped;                        
                    }
                }

                if (oldDirection != DirectionOfRotation)
                    FireStateChange();
            }
        }*/

        private static void MDeviceManager_OnConnectionChanged(DeviceManager.DeviceConnectionEventArgs args)
        {
            var qsb = args.Device as QSB_S;
            if (qsb != null)
            {
                if (args.IsConnected && (mQSB == null))
                {
                    mQSB = qsb;
                    ConfigQsb();
                }
                else if (args.Device == mQSB)
                {
                    mQSB = null;
                }
            }
        }

        private static void FireStopStateChange()
        {
            Messenger.Default.Send<QuadratureStateChangeMessage>(new QuadratureStateChangeMessage { DirectionOfRotation = DirectionOfRotation.Stopped, Timestamp = DateTime.Now.ToUniversalTime() });

            lock (eventQueue)
            {
                eventQueue.Enqueue(new QuadratureStateChangeMessage { DirectionOfRotation = DirectionOfRotation.Stopped, Timestamp = DateTime.Now.ToUniversalTime() });
                while (eventQueue.Count() > 5) eventQueue.Dequeue();
            }
        }

        private static void FireStateChange()
        {           
            Messenger.Default.Send<QuadratureStateChangeMessage>(new QuadratureStateChangeMessage { DirectionOfRotation = DirectionOfRotation, Timestamp = DateTime.Now.ToUniversalTime() });
            
            lock (eventQueue)
            {
                eventQueue.Enqueue(new QuadratureStateChangeMessage { DirectionOfRotation = DirectionOfRotation, Timestamp = DateTime.Now.ToUniversalTime() });
                while (eventQueue.Count() > 5) eventQueue.Dequeue();
            }
        }

        public static void ClearBuffer()
        {
            lock (eventQueue)
            {
                eventQueue.Clear();
            }
        }

        public static QuadratureStateChangeMessage LastEvent
        {
            get
            {
                lock (eventQueue)
                {
                    if (eventQueue.Count() == 0) return null;
                    else
                    {
                        return eventQueue.LastOrDefault();
                    }
                }
            }
        }

        public static QuadratureStateChangeMessage PreviousEvent
        {
            get
            {
                lock (eventQueue)
                {
                    if (eventQueue.Count() <= 1) return null;
                    else
                    {
                        return eventQueue.ToArray()[eventQueue.Count()-2];
                    }
                }
            }
        }

        public static void Dispose()
        {
            try
            {
                if (mQSB != null)
                {
                    //mQSB.OnRegisterValueChanged -= MQSB_OnRegisterValueChanged;
                    //mQSB.CancelStreaming();
                }

                if (mDeviceManager != null)
                {
                    mDeviceManager.Shutdown();
                }
            }
            catch(Exception exc)
            {
                System.Diagnostics.Trace.Write(exc.Message);
            }
        }
    }

    

    public class EncoderSample
    {
        public int PositionValue { get; set; }
        public DirectionOfRotation DirectionOfRotation { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
