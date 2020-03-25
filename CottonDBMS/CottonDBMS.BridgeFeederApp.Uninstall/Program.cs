/*
MIT License

Copyright (c) 2018  United States Department of Agriculture,  Agricultural Research Service

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
SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Diagnostics;
using CottonDBMS.DataModels;
using CottonDBMS.DataModels.Helpers;
using CottonDBMS.Cloud;
using System.Threading;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;

namespace CottonDBMS.BridgeFeederApp.Uninstall
{
    class Program
    {
        //short exe to remove database and startup info created by application
        private static void DeleteFile(string path)
        {
            try
            {
                Console.WriteLine("Deleting " + path);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.StackTrace);
            }
        }        

        private static void DoUninstall()
        {
            try
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                dir = dir.TrimEnd('\\') + "\\" + FolderConstants.ROOT_APP_DATA_FOLDER;
              
                string appDataFolder = dir + "\\" + FolderConstants.FEEDER_BRIDGE_APP_DATA_FOLDER;
                string appSyncFolder = dir + "\\" + FolderConstants.FEEDER_BRIDGE_SYNC_APP_DATA_FOLDER;

                if (System.IO.Directory.Exists(appDataFolder))
                    System.IO.Directory.Delete(appDataFolder, true);

                if (System.IO.Directory.Exists(appSyncFolder))
                    System.IO.Directory.Delete(appSyncFolder, true);

                Console.WriteLine("Files deleted.");

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.StackTrace);
                Console.WriteLine("Press enter to continue.");
                Console.ReadLine();
            }
        }

        static void Main(string[] args)
        {
            try
            {               
                DoUninstall();                
                System.Threading.Thread.Sleep(3500);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message + " " + exc.StackTrace);
                Console.WriteLine("Press any key to exit");
                Console.ReadLine();
            }
        }
    }
}
