//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CottonDBMS.GinApp.Helpers;
using CottonDBMS.DataModels;
using System.Diagnostics;

namespace CottonDBMS.GinApp
{
    static class Program
    {
        public static bool alreadyRunningProcess()
        {
            Process thisProcess = Process.GetCurrentProcess();
            Process[] matchingProcs = Process.GetProcessesByName(thisProcess.ProcessName);
            foreach (Process p in matchingProcs)
            {
                if ((p.Id != thisProcess.Id) &&
                    (p.MainModule.FileName == thisProcess.MainModule.FileName))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (alreadyRunningProcess()) return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
                        
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            dir = dir.TrimEnd('\\') + "\\" + FolderConstants.ROOT_APP_DATA_FOLDER;

            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }

            string appLogDir = dir + "\\"+FolderConstants.GIN_APP_DATA_FOLDER+"\\";
            if (!System.IO.Directory.Exists(appLogDir))
            {
                System.IO.Directory.CreateDirectory(appLogDir);
            }

            CottonDBMS.Logging.Logger.SetLogPath(appLogDir);

            AppDomain.CurrentDomain.SetData("DataDirectory", dir.TrimEnd('\\'));
            MainForm form = new MainForm();
            form.WindowState = FormWindowState.Minimized;          
            Application.Run(form);

        }
    }
}
