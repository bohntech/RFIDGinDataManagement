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
using CottonDBMS.Interfaces;
using CottonDBMS.GinApp.Helpers;
using CottonDBMS.DataModels;
using CottonDBMS.GinApp.Classes;
using System.Security.Permissions;
using System.IO;

namespace CottonDBMS.GinApp.Dialogs
{
    
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class AddEditPickupList : Form
    {
        #region private properties
        bool hasError = false;
        List<ModuleEntity> modulesSelected = new List<ModuleEntity>();
        PickupListEntity dataObj = null;

        List<string> downloadedByTrucks = new List<string>();
        List<string> modulesDownloaded = new List<string>();

        CheckBox checkAllModules = new CheckBox();
        CheckBox checkAllTrucks = new CheckBox();

        double selectedLat = 0.00;
        double selectedLng = 0.00;

        bool documentCompleted = false;
        bool bindingDestList = true;

        private bool ReadOnly
        {
            get
            {
                return false;
            }
        }

        private PickupListDestination SelectedDestination
        {
            get
            {
                BaseEntity dest = (BaseEntity)cboDestination.SelectedItem;
                return (PickupListDestination)Convert.ToInt32(dest.Id);
            }
        }
        #endregion

        #region private methods
        private void clearErrors()
        {
            hasError = false;
            clientSelector.ClearErrors();
            farmSelector.ClearErrors();
            fieldSelector.ClearErrors();
            errorProvider.SetError(tbListname, "");
        }

        private bool ValidateForm()
        {
            clearErrors();
            hasError = clientSelector.ValidateForm();

            bool farmError =  farmSelector.ValidateForm();
            bool fieldError = fieldSelector.ValidateForm();

            if (farmError || fieldError) hasError = true;

            ValidateListName();

            return !hasError;
        }

        private void ValidateListName()
        {
            if (string.IsNullOrWhiteSpace(tbListname.Text))
            {
                errorProvider.SetError(tbListname, "List name is requred.");
                hasError = true;
            }
            else
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    bool result = uow.PickupListRepository.CanSavePickupList((dataObj != null) ? dataObj.Id : string.Empty, tbListname.Text.Trim());
                    if (!result)
                    {
                        hasError = true;
                        errorProvider.SetError(tbListname, "List name already exists.");
                    }
                }
            }
        }

        private void WriteArchiveFileAsync()
        {
            try
            {
                string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                string path = ConfigHelper.ArchivePath;

                string fileName = string.Format("{0}_{1}_{2}_{3}_{4}.csv", clientSelector.ExistingOrNewClientName, farmSelector.ExistingOrNewFarmName, fieldSelector.ExistingOrNewFieldName, DateTime.Now.ToString("yyyyy_MM_dd_HH_mm_ss"), tbListname.Text.Trim());
                fileName = path + FileHelper.CleanFilename(fileName);

                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    dataObj = uow.PickupListRepository.GetById(dataObj.Id, "AssignedModules", "DownloadedToTrucks", "AssignedTrucks", "Field.Farm.Client");
                }

                using (StreamWriter sw = File.CreateText(fileName))
                {
                    //write header
                    sw.WriteLine("Client,Farm,Field,List,SerialNumber,AssignedTruckIDs,Latitude,Longitude");
                                        
                    string latString = "";
                    string longString = "";
                    foreach (var module in dataObj.AssignedModules)
                    {

                        latString = "";
                        longString = "";

                        if (module.Latitude > 0 || module.Latitude < 0) latString = module.Latitude.ToString();
                        if (module.Longitude > 0 || module.Longitude < 0) longString = module.Longitude.ToString();
                        
                        sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                            FileHelper.EscapeForCSV(dataObj.ClientName),
                            FileHelper.EscapeForCSV(dataObj.FarmName),
                            FileHelper.EscapeForCSV(dataObj.Field.Name),
                            FileHelper.EscapeForCSV(dataObj.Name),
                            FileHelper.EscapeForCSV(module.Name),
                            FileHelper.EscapeForCSV(dataObj.DownloadedByTruckNames),
                            latString,
                           longString
                            ));
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                MessageBox.Show("An error occurred writing archive file for pick up list.");
            }
        }
        #endregion

        #region private events
        private void Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in availableModulesGridView.Rows)
            {
                var module = (ModuleEntity)row.DataBoundItem;

                if (canEditModule(module))
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                    chk.Value = checkAllModules.Checked; //because chk.Value is initialy null  
                }
            }

            if (availableModulesGridView.Rows.Count > 0)
                availableModulesGridView.CurrentCell = availableModulesGridView.Rows[0].Cells[2];
        }

        private void TruckCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gridViewTrucks.Rows)
            {
                var truck = (TruckEntity)row.DataBoundItem;
                if (!downloadedByTrucks.Contains(truck.Id))
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                    chk.Value = checkAllTrucks.Checked; //because chk.Value is initialy null  
                }
            }

            if (gridViewTrucks.Rows.Count > 0)
                gridViewTrucks.CurrentCell = gridViewTrucks.Rows[0].Cells[1];
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            pnlModuleStep.Visible = true;
            pnlTruckStep.Visible = false;
        }

        private async Task RefreshDownloadedTrucks()
        {
            var truckDownloadDocs = await CottonDBMS.Cloud.DocumentDBContext.GetAllItemsAsync<TruckListsDownloaded>(t => t.EntityType == EntityType.TRUCK_LISTS_DOWNLOADED);            
            downloadedByTrucks = new List<string>();

            if (dataObj != null)
            {
                foreach (var doc in truckDownloadDocs)
                {
                    if (doc.PickupListsDownloaded.Contains(dataObj.Id))
                        downloadedByTrucks.Add(doc.Name); //add truck id to list of trucks with document
                }
            }
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            ValidateForm();
            if (!hasError)
            {
                //get checked modules
                modulesSelected.Clear();
                if (!mapBrowser.Visible)
                {
                    foreach (DataGridViewRow row in availableModulesGridView.Rows)
                    {
                        if (row.Cells[0].Value != null && (bool)row.Cells[0].Value)
                        {
                            modulesSelected.Add((ModuleEntity)row.DataBoundItem);
                        }
                    }
                }

                if (modulesSelected.Count() < 1 && availableModulesGridView.Visible) //there are modules available to select
                {
                    MessageBox.Show("Please check at least one module to add to the list.");
                    return;
                }

                if (mapBrowser.Visible && (selectedLng == 0 || selectedLat == 0)) //there are no modules to select so the map was shown
                {
                    MessageBox.Show("Please place a marker on the map to indicate the field location.");
                    return;
                }              

                pnlModuleStep.Visible = false;
                lblTruckInstructions.Visible = true;
                gridViewTrucks.DataSource = null;
                lblTruckInstructions.Text = "Loading trucks...";
                pnlTruckStep.Visible = true;                
                pnlTruckStep.Enabled = false;
                gridViewTrucks.Visible = false;

                //load trucks grid
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    //go directly to cloud to fetch downloaded 
                    
                    IEnumerable<TruckEntity> allTrucks = uow.TruckRepository.GetAll().OrderBy(t => t.Name).ToList();
                    await RefreshDownloadedTrucks();

                    if (allTrucks.Count() > 0)  //field has modules so show list
                    {
                        gridViewTrucks.AutoGenerateColumns = false;
                        gridViewTrucks.DataSource = allTrucks;
                        gridViewTrucks.Columns[0].ReadOnly = ReadOnly;
                    }
                    else
                    {
                        lblTruckInstructions.Text = "No trucks have been entered.  Please enter trucks in the Trucks tab before creating a pickup list.";
                        gridViewTrucks.Visible = false;
                        pnlTruckStep.Enabled = true;
                        btnSaveAndClose.Enabled = false;
                    }
                }
            }
        }

        private void btnCancelClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            var selectedTrucks = new List<TruckEntity>();
            await RefreshDownloadedTrucks();
            foreach (DataGridViewRow row in gridViewTrucks.Rows)
            {
                var truckItem = (TruckEntity)row.DataBoundItem;
                if (row.Cells[0].Value != null && (bool)row.Cells[0].Value) selectedTrucks.Add(truckItem);
            }

            if (selectedTrucks.Count() < 1)
            {
                MessageBox.Show("Please check at least one truck to send this list to.");
            }
            else
            {
                //check to see to make sure ids weren't remove that may have been downloaded while dialog was open
                if (downloadedByTrucks.Count() > 0 && !selectedTrucks.Any(t => downloadedByTrucks.Contains(t.Id)) && dataObj != null)
                {
                    MessageBox.Show("A truck downloaded this list after you began editting.  Close this list and re-open to edit.");
                    return;
                }
                try
                {                    
                    using (var uow = UnitOfWorkFactory.CreateUnitOfWork()) {
                        ClientEntity client = clientSelector.GetOrCreateClientEntity();
                        FarmEntity farm = farmSelector.GetOrCreateFarmEntity(client);
                        FieldEntity field = fieldSelector.GetOrCreateFieldEntity(farm);
                        BaseEntity dest = (BaseEntity)cboDestination.SelectedItem;
                        if (modulesSelected.Count() < 1)
                        {
                            field.Latitude = selectedLat;
                            field.Longitude = selectedLng;
                            uow.FieldRepository.Save(field);
                        }

                        PickupListEntity existingList = null;

                        if (dataObj == null)
                        {
                            existingList = new PickupListEntity();
                            existingList.Id = Guid.NewGuid().ToString();
                            existingList.DownloadedToTrucks = new List<TruckEntity>();
                            existingList.AssignedModules = modulesSelected;
                            existingList.Created = DateTime.UtcNow;
                            existingList.AssignedTrucks = selectedTrucks;
                            existingList.FieldId = field.Id;
                            existingList.Name = tbListname.Text.Trim();
                            
                            existingList.Destination =  (PickupListDestination) Convert.ToInt32(dest.Id);
                            existingList.OriginalSerialNumbers = "";
                            foreach(var s in modulesSelected)
                            {
                                existingList.OriginalSerialNumbers += "," + s.Name;
                            }
                            existingList.OriginalSerialNumbers.TrimEnd(',');

                            existingList.OriginalModuleCount = modulesSelected.Count();

                            if (modulesSelected.Count() > 0)
                            {
                                var m = modulesSelected.First();
                                existingList.Latitude = m.Latitude;
                                existingList.Longitude = m.Longitude;
                            }
                            else
                            {
                                existingList.Latitude = selectedLat;
                                existingList.Longitude = selectedLng;
                            }
                            uow.PickupListRepository.Add(existingList);
                        }
                        else
                        {                            
                            existingList = uow.PickupListRepository.GetById(dataObj.Id, "AssignedModules", "AssignedTrucks", "DownloadedToTrucks");
                            existingList.FieldId = field.Id;
                            existingList.Name = tbListname.Text.Trim();
                            existingList.Latitude = selectedLat;
                            existingList.Longitude = selectedLng;
                            existingList.Destination = (PickupListDestination)Convert.ToInt32(dest.Id);

                            existingList.OriginalSerialNumbers = "";
                            foreach (var s in modulesSelected)
                            {
                                existingList.OriginalSerialNumbers += "," + s.Name;
                            }
                            existingList.OriginalSerialNumbers.TrimEnd(',');
                            existingList.OriginalModuleCount = modulesSelected.Count();
                            uow.PickupListRepository.Update(existingList, modulesSelected.Select(m => m.Id).ToList(), selectedTrucks.Select(t => t.Id).ToList());
                        }                       
                        
                        uow.SaveChanges();
                        dataObj = existingList;
                        WriteArchiveFileAsync();

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                catch (Exception exc)
                {
                    Logging.Logger.Log(exc);
                    MessageBox.Show("An error occurred trying to save pickup list. " + exc.Message);
                }
            }
        }

        private async void clientSelector_SelectionChanged(object sender, EventArgs e)
        {

            if (!clientSelector.ExistingSelected && !clientSelector.IsNew)
            {
                availableModulesGridView.Visible = false;
                mapBrowser.Visible = false;
                lblCheckInfo.Visible = false;
            }

            if (clientSelector.ExistingSelected || clientSelector.IsNew)
            {
                farmSelector.Visible = true;
                await farmSelector.Initialize(errorProvider, true, clientSelector.IsNew, clientSelector.SelectedClientId, (dataObj != null) ? dataObj.Field.Farm.Id : "");
            }
            else
            {
                pnlModuleStep.Enabled = true;
                farmSelector.Visible = false;
                fieldSelector.Visible = false;
            }
        }        

        private async void farmSelector_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (!farmSelector.ExistingSelected && !farmSelector.IsNew)
                {
                    availableModulesGridView.Visible = false;
                    mapBrowser.Visible = false;
                    lblCheckInfo.Visible = false;
                }

                if (farmSelector.ExistingSelected || farmSelector.IsNew)
                {
                    fieldSelector.Visible = true;
                    await fieldSelector.Initialize(errorProvider, true, farmSelector.IsNew, farmSelector.SelectedFarmId, (dataObj != null) ? dataObj.FieldId : "");
                }                
                else
                {
                    pnlModuleStep.Enabled = true;
                    fieldSelector.Visible = false;
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                throw new Exception("Error initalizing field drop down.", exc);
            }
        }

        private void fieldSelector_SelectionChanged(object sender, EventArgs e)
        {
            initList();
        }

        private void initList()
        {
            //load assigned modules into grid
            PickupListDestination dest = (PickupListDestination)Convert.ToInt32(((BaseEntity)cboDestination.SelectedItem).Id);
            IEnumerable<ModuleEntity> availableFieldModules = null;

            if (!fieldSelector.ExistingSelected && !fieldSelector.IsNew)
            {
                mapBrowser.Visible = false;
                lblCheckInfo.Visible = false;
                pnlTruckStep.Visible = false;
                availableModulesGridView.Visible = false;
                return;
            }

            if (fieldSelector.ExistingSelected)
            {
                var listId = (dataObj != null) ? dataObj.Id : "";
                availableModulesGridView.Visible = false;
                lblCheckInfo.Visible = true;
                lblCheckInfo.Text = "Loading modules...";

                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    availableFieldModules = uow.PickupListRepository.GetAvailableModulesForPickupList(listId, fieldSelector.SelectedFieldId, dest);
                }
            }

            if (availableFieldModules != null && availableFieldModules.Count() > 0)  //field has modules so show list
            {

                availableModulesGridView.DataSource = null;
                availableModulesGridView.AutoGenerateColumns = false;
                availableModulesGridView.DataSource = availableFieldModules;
                availableModulesGridView.Columns[0].ReadOnly = ReadOnly;
                mapBrowser.Visible = false;
                lblCheckInfo.Visible = true;
                pnlTruckStep.Visible = false;
            }
            else //field doesn't have any modules in the system or we're creating a new field
            {
                string currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                currentDir = currentDir.TrimEnd('\\') + "\\Html";

                string appDataGinDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).TrimEnd('\\') + "\\" + FolderConstants.ROOT_APP_DATA_FOLDER + "\\"
                    + FolderConstants.GIN_APP_DATA_FOLDER;

                //read html file
                string htmlSource = System.IO.File.ReadAllText(currentDir + "\\EmbeddedMap.html");
                string htmlSourceWithMapsKey = htmlSource.Replace("{MAPS_KEY}", ConfigHelper.GoogleMapsKey);
                System.IO.File.WriteAllText(appDataGinDir + "\\EmbeddedMapWithKey.html", htmlSourceWithMapsKey);
                mapBrowser.ObjectForScripting = this;
                this.mapBrowser.Url = new Uri(String.Format("file:///{0}/EmbeddedMapWithKey.html?id=" + Guid.NewGuid().ToString(), appDataGinDir));

                //need to send default coordinates for field if available if not send gin yard coordinates.
                mapBrowser.Visible = true;
                lblCheckInfo.Visible = true;
                availableModulesGridView.Visible = false;
                mapBrowser.DocumentCompleted -= mapBrowser_DocumentCompleted;
                documentCompleted = false;
                mapBrowser.DocumentCompleted += mapBrowser_DocumentCompleted;
            }

            pnlModuleStep.Enabled = true;
        }

        private void tbListname_Validating(object sender, CancelEventArgs e)
        {
            ValidateListName();
        }        

        private void mapBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (documentCompleted == false) {

                documentCompleted = true;
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    //get location of selected field
                    var field = uow.FieldRepository.GetById(fieldSelector.SelectedFieldId);
                    lblCheckInfo.Text = "Click a point on the map to set the pickup location";
                    List<object> parms = new List<object>();
                    if (field != null && field.Latitude != 0 && field.Longitude != 0)
                    {
                        parms.Add(field.Latitude);
                        parms.Add(field.Longitude);
                        selectedLat = field.Latitude;
                        selectedLng = field.Longitude;
                    }
                    else if (ConfigHelper.GinYardNWCornerLat != 0 && ConfigHelper.GinYardNWCornerLong != 0)
                    {
                        selectedLat = ConfigHelper.GinYardNWCornerLat;
                        selectedLng = ConfigHelper.GinYardNWCornerLong;
                    }
                    else
                    {
                        selectedLat = 33.5778631;
                        selectedLng = -101.8551665;

                    }
                    parms.Add(selectedLat);
                    parms.Add(selectedLng);

                    mapBrowser.Document.InvokeScript("setPinLocation", parms.ToArray());
                    if (ReadOnly)
                    {
                        mapBrowser.Document.InvokeScript("setReadOnly");
                        //lblCheckInfo.Text = "This list is currently locked because it has already been downloaded.";
                    }
                    mapBrowser.Document.InvokeScript("initMap");
                }
                            
            }
        }

        private void AddEditPickupList_Shown(object sender, EventArgs e)
        {
            LoadForm();
            tbListname.Focus();
            clearErrors();
        }

        private void availableModulesGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            availableModulesGridView.Visible = true;

           //if (dataObj == null || (dataObj != null && dataObj.DownloadedToTrucks.Count() <= 0))
            //{
                lblCheckInfo.Text = "Please check the module to include in this list.";
            //}
            //else
            //{
                //lblCheckInfo.Text = "This list is locked because it has been downloaded to a truck.";
            //}

            if (dataObj != null)
            {
                foreach (DataGridViewRow row in availableModulesGridView.Rows)
                {
                    ModuleEntity module = (ModuleEntity)row.DataBoundItem;
                    if (dataObj.AssignedModules.Any(m => m.Id == module.Id)) row.Cells[0].Value = true;

                    if (!canEditModule(module))  //lock this item so it can't be unchecked
                    {
                        DataGridViewCell cell = row.Cells[0];
                        DataGridViewCheckBoxCell chkCell = cell as DataGridViewCheckBoxCell;
                        chkCell.Value = true;
                        chkCell.FlatStyle = FlatStyle.Flat;
                        chkCell.Style.ForeColor = Color.DarkGray;
                        cell.ReadOnly = true;                                                
                    }

                }
            }
        }

        private void gridViewTrucks_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {                       
            lblTruckInstructions.Text = "Check each truck that should receive a copy of this list.";           

            if (dataObj != null)
            {
                foreach (DataGridViewRow row in gridViewTrucks.Rows)
                {
                    TruckEntity truck = (TruckEntity)row.DataBoundItem;
                    if (dataObj.AssignedTrucks.Any(t => t.Id == truck.Id)) row.Cells[0].Value = true;

                    if (downloadedByTrucks.Contains(truck.Id))
                    {
                        DataGridViewCell cell = row.Cells[0];
                        DataGridViewCheckBoxCell chkCell = cell as DataGridViewCheckBoxCell;
                        chkCell.Value = true;
                        chkCell.FlatStyle = FlatStyle.Flat;
                        chkCell.Style.ForeColor = Color.DarkGray;
                        cell.ReadOnly = true;
                    }
                }
            }

            gridViewTrucks.Visible = true;
            pnlTruckStep.Enabled = true;
        }
        #endregion

        #region public methods
        public void LoadForm()
        {            
            try
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    if (dataObj != null)
                    {                       
                       
                        //need to refresh pickup list document because it could have changed
                        dataObj = uow.PickupListRepository.GetById(dataObj.Id, new string[] {"Field.Farm.Client", "AssignedModules", "DownloadedToTrucks", "AssignedTrucks" });

                        if (ReadOnly)
                        {
                            farmSelector.Enabled = false;
                            fieldSelector.Enabled = false;
                            clientSelector.Enabled = false;
                            btnSaveAndClose.Enabled = false;
                            availableModulesGridView.ReadOnly = true;
                            gridViewTrucks.ReadOnly = true;
                            checkAllModules.Visible = false;
                            checkAllTrucks.Visible = false;
                            tbListname.ReadOnly = true;
                        }
                    }

                    if (dataObj != null)
                    {
                        tbListname.Text = dataObj.Name;
                        BindingHelper.BindPickupListDestinationCombo(cboDestination, dataObj.Destination);
                    }
                    else
                    {
                        tbListname.Text = string.Empty;
                        BindingHelper.BindPickupListDestinationCombo(cboDestination);
                    }
                    bindingDestList = false;

                    clientSelector.Initialize(errorProvider, true, false, (dataObj != null) ? dataObj.Field.Farm.Client.Id : "");
                    farmSelector.FormErrorProvider = errorProvider;
                    fieldSelector.FormErrorProvider = errorProvider;
                    
                    clearErrors();
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                throw new Exception("Error occurred loading pickup list form.", exc);
            }
        }

        public DialogResult ShowAdd()
        {          
            this.Text = "Add Pickup List";
            mapBrowser.Visible = false;
            lblCheckInfo.Visible = false;
            pnlTruckStep.Visible = false;
            pnlModuleStep.Visible = true;
            availableModulesGridView.Visible = false;
            return this.ShowDialog();
        }

        public async Task<DialogResult> ShowEdit(PickupListEntity doc)
        {
            dataObj = doc;
            this.Text = "Update Pickup List";

            await RefreshDownloadedTrucks();

            //has list already been downloaded if so prevent removing modules 
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                var thisList = uow.PickupListRepository.GetById(doc.Id, "AssignedModules", "AssignedTrucks", "DownloadedToTrucks");
                if (downloadedByTrucks.Count() > 0)
                    modulesDownloaded.AddRange(thisList.AssignedModules.Select(s => s.Id).ToList());
            }

            return this.ShowDialog();
        }

        public void SetCoords(object lat, object lng)
        {
            selectedLat = (double)lat;
            selectedLng = (double)lng;
        }
        #endregion


        public AddEditPickupList()
        {
            InitializeComponent();

            checkAllModules.CheckedChanged += Checkbox_CheckedChanged;
            checkAllTrucks.CheckedChanged += TruckCheckbox_CheckedChanged;

            checkAllModules.Size = new System.Drawing.Size(15, 15);
            checkAllModules.BackColor = Color.Transparent;
            checkAllModules.Padding = new Padding(0);
            checkAllModules.Margin = new Padding(0);
            checkAllModules.Text = "";

            checkAllTrucks.Size = new System.Drawing.Size(15, 15);
            checkAllTrucks.BackColor = Color.Transparent;
            checkAllTrucks.Padding = new Padding(0);
            checkAllTrucks.Margin = new Padding(0);
            checkAllTrucks.Text = "";

            availableModulesGridView.Controls.Add(checkAllModules);
            availableModulesGridView.Visible = false;
            lblCheckInfo.Text = "Loading modules...";
            lblTruckInstructions.Text = "Loading trucks...";

            pnlModuleStep.Enabled = false;
            pnlTruckStep.Enabled = false;

            DataGridViewHeaderCell headerModules = availableModulesGridView.Columns[0].HeaderCell;
            checkAllModules.Location = new Point(
                headerModules.ContentBounds.Left + (headerModules.ContentBounds.Right - headerModules.ContentBounds.Left + checkAllModules.Width) / 2 + 43,
                headerModules.ContentBounds.Top + (headerModules.ContentBounds.Bottom - headerModules.ContentBounds.Top + checkAllModules.Size.Height) / 2 - 13 
            );

            gridViewTrucks.Controls.Add(checkAllTrucks);
            DataGridViewHeaderCell headerTrucks = gridViewTrucks.Columns[0].HeaderCell;
            checkAllTrucks.Location = new Point(
                headerTrucks.ContentBounds.Left + (headerTrucks.ContentBounds.Right - headerTrucks.ContentBounds.Left + checkAllTrucks.Width) / 2 + 53,
                headerTrucks.ContentBounds.Top + (headerTrucks.ContentBounds.Bottom - headerTrucks.ContentBounds.Top + checkAllTrucks.Size.Height) / 2 
            );
        }

        private bool canEditModule(ModuleEntity m)
        {
            bool edittableStatus = (m.ModuleStatus == ModuleStatus.IN_FIELD && SelectedDestination == PickupListDestination.GIN_YARD) ||
                                   (m.ModuleStatus == ModuleStatus.AT_GIN && SelectedDestination == PickupListDestination.GIN_FEEDER);            


            if (!edittableStatus || (modulesDownloaded.Contains(m.Id) && dataObj != null))
            {
                return false;
            }
            else return true;
        }

        private void availableModulesGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var module = (ModuleEntity)availableModulesGridView.Rows[e.RowIndex].DataBoundItem;

            if (!canEditModule(module))
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void gridViewTrucks_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var truck = (TruckEntity)gridViewTrucks.Rows[e.RowIndex].DataBoundItem;

            if (downloadedByTrucks.Contains(truck.Id) && dataObj != null)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void cboDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!bindingDestList)
            {
                initList();
            }
        }
    }
}
