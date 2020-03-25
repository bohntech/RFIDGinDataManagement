//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CottonDBMS.GinApp.Dialogs;

namespace CottonDBMS.GinApp.Classes
{
    public static class BusyMessage
    {
        public static bool open = false;
        
        public static event EventHandler OnBusyMessageShown;

        public static event EventHandler OnBusyMessageClosed;

        private static BusyDialog dialog =  new BusyDialog();

        public static void Show(string message, Form theForm)
        {
            if (open)
                UpdateMessage(message);

            if (theForm.InvokeRequired)
            {
                theForm.Invoke((MethodInvoker)delegate
                {
                    Show(message, theForm);
                });
            }
            else
            {
               
                    //if (dialog == null)
                    //    dialog = new BusyDialog();

                    open = true;
                    dialog.ShowMessage(message, theForm);
               
                if (OnBusyMessageShown != null)
                {
                    OnBusyMessageShown(null, new EventArgs());
                }
            }
        }

        public static void UpdateMessage(string message)
        {
            if (dialog.InvokeRequired)
            {
                dialog.Invoke((MethodInvoker)delegate
                {
                    UpdateMessage(message);
                });
            }
            else
            {
              
                    dialog.UpdateMessage(message);
                
            }
        }

        public static void Close()
        {
            if (dialog.InvokeRequired)
            {
                dialog.Invoke((MethodInvoker)delegate
                {
                    Close();
                });
            }
            else
            {
                open = false;
                dialog.Hide();

                if (OnBusyMessageClosed != null)
                {
                    OnBusyMessageClosed(null, new EventArgs());
                }
            }
        }
    }
}
