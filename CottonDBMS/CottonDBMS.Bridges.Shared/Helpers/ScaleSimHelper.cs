using CottonDBMS.Bridges.Shared.Messages;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CottonDBMS.Bridges.Shared.Helpers
{
    public class ScaleSimHelper
    {
        public static void RunSim()
        {
            bool CtrlDown = System.Windows.Input.Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            bool ZeroDown = Keyboard.IsKeyDown(Key.NumPad0);
            bool OneDown = Keyboard.IsKeyDown(Key.NumPad1);
            Task.Run(() =>
            {
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<KeyDownMessage>(new KeyDownMessage { });
                if (CtrlDown)
                {
                    string filesPath = AppDomain.CurrentDomain.BaseDirectory + "\\Files\\";
                    string enterScalePath = filesPath + "EnterScale1.txt";
                    string exitScalePath = filesPath + "ExitScale2.txt";
                    string fileToPlay = "";

                    if (ZeroDown)
                    {
                        fileToPlay = enterScalePath;
                    }
                    else if (OneDown)
                    {
                        fileToPlay = exitScalePath;
                    }

                    if (!string.IsNullOrEmpty(fileToPlay))
                    {
                        // Task.Run(() =>
                        //{
                        try
                        {
                            var lines = System.IO.File.ReadAllLines(fileToPlay);
                            SerialPort mySerialPort = new SerialPort("COM14", 9600, Parity.None, 8, StopBits.One);
                            mySerialPort.Open();
                            mySerialPort.Handshake = Handshake.None;
                            byte[] data = null;
                            foreach (var line in lines)
                            {
                                data = Encoding.ASCII.GetBytes(line.TrimEnd() + "\r\n");
                                mySerialPort.Write(data, 0, data.Length);
                                System.Threading.Thread.Sleep(25);

                            }
                            mySerialPort.Close();
                        }
                        catch (Exception exc)
                        {
                            CottonDBMS.Logging.Logger.Log(exc);
                        }
                        // });
                    }
                }
            });
        }
    }
}
