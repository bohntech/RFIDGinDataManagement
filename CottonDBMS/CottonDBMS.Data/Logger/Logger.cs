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

        public static void SetLogPath(string path)
        {
            lock (_locker)
            {
                _path = path;
            }
        }

        public static void Log(string type, string message)
        {
            lock (_locker)
            {
                try
                {
                    string contents = string.Format("{0} {1} {2}\r\n", DateTime.Now.ToString(), type.PadRight(10), message);
                    File.AppendAllText(_path.TrimEnd('\\') + "\\" + _fileName, contents);
                }
                catch (Exception exc)
                {
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

        public static void CleanUp()
        {
            lock (_locker)
            {
                string filepath = _path.TrimEnd('\\') + "\\" + _fileName;
                if (File.Exists(filepath))
                {
                    FileInfo info = new FileInfo(filepath);

                    if (info.Length > (1024 * 1024 * 15)) //truncate log file if it exceeds 15MB
                    {
                        File.Delete(filepath);
                    }
                }
            }
        }
    }
}
