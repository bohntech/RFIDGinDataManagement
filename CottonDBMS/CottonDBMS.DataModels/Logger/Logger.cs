/*
The MIT License(MIT)

Copyright(c) 2016 Bohn Technology Solutions, LLC

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. */

/* Re-used logger code from Cotton Harvest Download Utility - Modified namespace */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CottonDBMS.Logging
{
    /// <summary>
    /// This is a simple logger class used to record.  Diagnostic information in the application.
    /// </summary>
    public static class Logger
    {
        private static string _fileName = "log.txt";
        private static string _path = "";
        private static string _locker = "";

        private static List<string> bufferedMessages = new List<string>();

        public static void SetLogPath(string path)
        {
            lock (_locker)
            {
                _path = path;

                if (!File.Exists(_path.TrimEnd('\\') + "\\" + _fileName))
                {
                    File.CreateText(_path.TrimEnd('\\') + "\\" + _fileName);
                }
            }
        }

        public static void WriteBuffer()
        {
            bufferedMessages.Add("WRITING BUFFER");
            
            File.AppendAllLines(_path.TrimEnd('\\') + "\\" + _fileName, bufferedMessages);          

            bufferedMessages.Clear();
        }

        public static void Log(string type, string message)
        {
            lock (_locker)
            {
                try
                {
                    string contents = string.Format("{0} {1} {2} {3}\r\n", System.Threading.Thread.CurrentThread.ManagedThreadId.ToString().PadLeft(10, ' '), DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:fff"), type.PadRight(10), message);
                    //Console.WriteLine(contents);
                    bufferedMessages.Add(contents);                    
                    if (bufferedMessages.Count() > 2000) CleanUpNoLoc(); 
                }
                catch (Exception exc)
                {
                    string s = exc.Message;
                    //do nothing we just want to make sure the logger never creates an exception
                }
            }
        }
        public static string CurrentLogFile
        {
            get
            {
                return _path.TrimEnd('\\') + "\\" + _fileName;
            }
        }

        public static void Log(Exception exc)
        {
            Log("ERROR", exc.Message);
            Log("TRACE", exc.StackTrace);
        }

        private static void CleanUpNoLoc()
        {
            try
            {
                string filepath = _path.TrimEnd('\\') + "\\" + _fileName;

                WriteBuffer();

                if (File.Exists(filepath))
                {
                    FileInfo info = new FileInfo(filepath);

                    if (info.Length > (1024 * 1024 * 50)) //truncate log file if it exceeds 50MB
                    {
                        File.Delete(filepath);
                    }
                }
            }
            catch (Exception exc)
            {
                //can't really do anything because an exception here means logging might not work
                string s = exc.Message;
            }
        }

        public static void CleanUp()
        {
            lock (_locker)
            {
                CleanUpNoLoc();
            }
        }
    }
}
