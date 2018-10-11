//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using CottonDBMS.Cloud;
using System.IO;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using MimeKit;

namespace CottonDBMS.GinApp.Classes
{
    

    public class MailImportTask : IDisposable
    {        
        private string _sourceDirectory = string.Empty;

        public event ProgressHandler OnProgressUpdate;
        public EventArgs e = null;

        private string hostname = "";
        private string password = "";
        private string username = "";
        private int port = 0;
        

        private void updateStatus(string msg)
        {
            if (OnProgressUpdate != null)
            {
                OnProgressUpdate(msg);
            }
        }      

        public MailImportTask(string _host, string _password, string _port, string _username, string _folder)
        {
            hostname = _host;
            password = _password;
            int.TryParse(_port, out port);
            username = _username;
            _sourceDirectory = _folder;
            System.IO.Directory.CreateDirectory(_sourceDirectory);
        }

        public bool Run(DateTime startDate)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hostname) && !string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(username) && port != 0)
                {
                    using (var client = new ImapClient())
                    {                                                
                        client.Connect(hostname, port, true);
                        client.CheckCertificateRevocation = true;                                                          

                        // Note: since we don't have an OAuth2 token, disable
                        // the XOAUTH2 authentication mechanism.
                        client.AuthenticationMechanisms.Remove("XOAUTH2");

                        client.Authenticate(username, password);

                        // The Inbox folder is always available on all IMAP servers...
                        var inbox = client.Inbox;
                        inbox.Open(FolderAccess.ReadOnly);
                        var query = SearchQuery.DeliveredAfter(startDate);
                        var results = inbox.Search(query);

                        for (int i = 0; i < results.Count; i++)
                        {
                            var message = inbox.GetMessage(results[i]);

                            if (message.Date.ToUniversalTime() > startDate.ToUniversalTime())
                            {
                                updateStatus(string.Format("Inspecting email {0} of {1} for module attachments.", (i + 1), results.Count));

                                foreach (var attachment in message.Attachments)
                                {
                                    var fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;

                                    if (fileName.ToLower().IndexOf("transmission") >= 0 && !File.Exists(_sourceDirectory + fileName))
                                    {
                                        fileName = DateTime.Now.ToString("MM_dd_yyyy_hh_mm_ss_ff") + "_" + fileName;
                                        using (var stream = File.Create(_sourceDirectory + fileName))
                                        {
                                            if (attachment is MessagePart)
                                            {
                                                var rfc822 = (MessagePart)attachment;

                                                rfc822.Message.WriteTo(stream);
                                            }
                                            else
                                            {
                                                var part = (MimePart)attachment;

                                                part.ContentObject.DecodeTo(stream);
                                            }

                                            stream.Flush();
                                            stream.Close();
                                            System.Threading.Thread.Sleep(20);
                                        }
                                    }
                                }
                            }
                        }

                        client.Disconnect(true);
                    }
                }

                return true;
            }
            catch(Exception exc)
            {
                Logging.Logger.Log(exc);
                return false;
            }
        }

        public void Dispose()
        {

        }
    }
}
