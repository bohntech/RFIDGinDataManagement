﻿/*
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
using CottonDBMS.EF;
using CottonDBMS.Cloud;
using System.Threading;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;

namespace CottonDBMS.GinApp.Uninstall
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

        private static async Task DeleteCollection(string appDataPath)
        {
            try
            {
                string settingsFile = appDataPath + "\\"+FolderConstants.GIN_APP_DATA_FOLDER+"\\settings.txt";
                string encryptedString = System.IO.File.ReadAllText(settingsFile);
                string decryptedString = CottonDBMS.Helpers.EncryptionHelper.Decrypt(encryptedString);
                var parms = Newtonsoft.Json.JsonConvert.DeserializeObject<TruckAppInstallParams>(decryptedString);

                if (!string.IsNullOrEmpty(parms.EndPoint)
                    && !string.IsNullOrEmpty(parms.Key))
                {
                    DocumentDBContext.Initialize(parms.EndPoint, parms.Key);

                    //remove document tracking lists this truck has downloaded
                    //this allows them to be released for delete etc at the gin            
                    await DocumentDBContext.DeleteCollectionAsync();
                }
                else
                {
                    Console.WriteLine("One or more settings null.");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.StackTrace);
                Console.WriteLine("Press enter to continue.");
                Console.ReadLine();
            }
        }

        private static async Task DoUninstall()
        {
            try
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                dir = dir.TrimEnd('\\') + "\\" + FolderConstants.ROOT_APP_DATA_FOLDER;

                await DeleteCollection(dir);

                string file1 = dir + "\\CottonDBMSGinDB.mdf";
                string file2 = dir + "\\CottonDBMSGinDB_log.ldf";

                string ginAppDataFolder = dir + "\\" + FolderConstants.GIN_APP_DATA_FOLDER;

                if (System.IO.Directory.Exists(ginAppDataFolder))
                    System.IO.Directory.Delete(ginAppDataFolder, true);             

                DeleteFile(file1);
                DeleteFile(file2);
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
                //check for a command line argument this
                //prevents someone from launching by just clicking the executable file
                //should only be launched as custom action from uninstaller
                if (args.Length == 1 && args[0] == "1")
                {
                    Task callTask = Task.Run(async () =>
                    {
                        System.Threading.Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                        await DoUninstall();
                    });
                    callTask.Wait();
                }
                else
                {
                    Console.WriteLine("No command line argument specified.");
                    Console.ReadLine();
                }
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
