//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using CottonDBMS.GinApp.Helpers;
using CottonDBMS.GinApp.Classes;

namespace CottonDBMS.GinApp.Dialogs
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class MapReportDialog : Form
    {
        int downloaded = 0;
        int total = 0;
        List<ModulePoint> modulePoints = new List<ModulePoint>();        
        ModuleFilter filter = new ModuleFilter();      
        public string ImageFilename { get; set; }
        bool documentCompleted = false;

        public MapReportDialog()
        {
            InitializeComponent();

            BusyMessage.OnBusyMessageClosed += BusyMessage_OnBusyMessageClosed;
            BusyMessage.OnBusyMessageShown += BusyMessage_OnBusyMessageShown;
        }

        private void BusyMessage_OnBusyMessageShown(object sender, EventArgs e)
        {
            this.Enabled = false;
        }

        private void BusyMessage_OnBusyMessageClosed(object sender, EventArgs e)
        {
            this.Enabled = true;
        }

        public void SetFilter(ModuleFilter searchFilter)
        {
            filter = searchFilter;
        }

       

        private void btnSaveToPdf_Click(object sender, EventArgs e)
        {
            this.BringToFront();
            Bitmap b = new Bitmap(mapBrowser.ClientSize.Width, mapBrowser.ClientSize.Height);
            Graphics g = Graphics.FromImage(b);
            g.CopyFromScreen(mapBrowser.Parent.PointToScreen(mapBrowser.Location), new Point(0, 0), mapBrowser.ClientSize);
            //The bitmap is ready. Do whatever you please with it!
            b.Save(ImageFilename);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCloseMap_Click(object sender, EventArgs e)
        {

        }

        private void MapReportDialog_Load(object sender, EventArgs e)
        {

        }

        private void RenderMap()
        {
            string currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            currentDir = currentDir.TrimEnd('\\') + "\\Html";

            string appDataGinDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).TrimEnd('\\') + "\\" + FolderConstants.ROOT_APP_DATA_FOLDER + "\\"
                    + FolderConstants.GIN_APP_DATA_FOLDER;

            //read html file
            string htmlSource = System.IO.File.ReadAllText(currentDir + "\\ReportMap.html");
            string htmlSourceWithMapsKey = htmlSource.Replace("{MAPS_KEY}", ConfigHelper.GoogleMapsKey);
            System.IO.File.WriteAllText(appDataGinDir + "\\ReportMapWithKey.html", htmlSourceWithMapsKey);
            mapBrowser.ObjectForScripting = this;
            this.mapBrowser.Url = new Uri(String.Format("file:///{0}/ReportMapWithKey.html?id=" + Guid.NewGuid().ToString(), appDataGinDir));
            mapBrowser.DocumentCompleted -= mapBrowser_DocumentCompleted;
            documentCompleted = false;
            mapBrowser.DocumentCompleted += mapBrowser_DocumentCompleted;
        }        

        private void mapBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (!documentCompleted)
            {
                documentCompleted = true;
                try
                {

                    List<object> parms = new List<object>();
                    parms.Add(modulePoints.Average(x => x.Latitude));
                    parms.Add(modulePoints.Average(x => x.Longitude));
                    parms.Add(18);
                    mapBrowser.Document.InvokeScript("initMap", parms.ToArray());
                    mapBrowser.Visible = true;
                    //iterate all datapoints and add to map                
                    foreach (var point in modulePoints)
                    {
                        parms.Clear();
                        parms.Add(point.Latitude);
                        parms.Add(point.Longitude);
                        parms.Add(point.SerialNumber);
                        parms.Add(point.ModuleStatus);
                        mapBrowser.Document.InvokeScript("addPoint", parms.ToArray());
                    }
                    mapBrowser.Document.InvokeScript("fitBounds");
                    BusyMessage.Close();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                    BusyMessage.Close();
                }
                this.Focus();
                this.BringToFront();
            }
        }

        private async void MapReportDialog_Shown(object sender, EventArgs e)
        {
            BusyMessage.Show("Loading...", this.FindForm());
            try
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    mapBrowser.Visible = false;
                    downloaded = 0;
                    total = 0;
                    modulePoints.Clear();

                    BusyMessage.UpdateMessage("Loading module locations");

                    if (filter.RecentOnly)
                        modulePoints.AddRange(uow.ModuleRepository.GetModulePoints(filter));
                    else
                        modulePoints.AddRange(uow.ModuleRepository.GetModulePointHistory(filter));
                                      
                    BusyMessage.UpdateMessage("Plotting module locations");
                }
            }
            catch(Exception exc)
            {
                Logging.Logger.Log(exc);
                BusyMessage.Close();
            }

            RenderMap();
        }
    }
}
