//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using System.IO;

namespace CottonDBMS.GinApp.Classes
{
    public delegate void ProgressHandler(string statusMessage);

    public class LocalFileImportTask : IDisposable
    {
        private DateTime? _lastImport = null;
        private string[] _paths;

        public event ProgressHandler OnProgressUpdate;
        public EventArgs e = null;
        
        private Dictionary<string, bool> existingSerialNumbers = new Dictionary<string, bool>(50000);      

        int clientColIndex = -1;
        int farmColIndex = -1;
        int fieldColIndex = -1;
        int loadColIndex = -1;
        int serialNumberColIndex = -1;
        int driverColIndex = -1;
        int truckColIndex = -1;
        int latitudeColIndex = -1;
        int longitudeColIndex = -1;
        int commentsColIndex = -1;
        int moduleIdColIndex = -1;
        int clientIDColIndex = -1;
        int farmIDColIndex = -1;
        int fieldIDColIndex = -1;

        DateTime? lastStatusUpdate = null;

        private void updateStatus(string msg, bool forceUpdate)
        {
            //update status every second so we don't flood the UI with more updates than it can handle
            if (forceUpdate || !lastStatusUpdate.HasValue || lastStatusUpdate.Value.AddSeconds(1) < DateTime.Now)
            {
                if (OnProgressUpdate != null)
                {
                    OnProgressUpdate(msg);
                }
                lastStatusUpdate = DateTime.Now;
            }
        }

        private bool processRow(CSVRow row, CSVFileParser parser)
        {
            try
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {

                    //create a new module document
                    ModuleEntity doc = new ModuleEntity();                  
                    doc.Name = row.GetColumnValue(serialNumberColIndex);
                    string clientName = row.GetColumnValue(clientColIndex);
                    string farmName = row.GetColumnValue(farmColIndex);
                    string fieldName = row.GetColumnValue(fieldColIndex);
                    doc.ImportedLoadNumber = row.GetColumnValue(loadColIndex);
                    doc.Driver = row.GetColumnValue(driverColIndex);
                    doc.TruckID = row.GetColumnValue(truckColIndex);
                    doc.Latitude = row.GetDoubleColumnValue(latitudeColIndex);
                    doc.Longitude = row.GetDoubleColumnValue(longitudeColIndex);
                    doc.Notes = row.GetColumnValue(commentsColIndex);
                    doc.ModuleId = row.GetColumnValue(moduleIdColIndex);
                    doc.Name = doc.Name.Trim();

                    string clientID = row.GetColumnValue(clientIDColIndex);
                    string farmID = row.GetColumnValue(farmIDColIndex);
                    string fieldID = row.GetColumnValue(fieldIDColIndex);

                    ModuleHistoryEntity historyItem = new ModuleHistoryEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Created = DateTime.UtcNow,
                        Driver = doc.Driver,
                        TruckID = doc.TruckID,
                        Latitude = doc.Latitude,
                        Longitude = doc.Longitude,
                        ModuleEventType = ModuleEventType.IMPORTED_FROM_FILE
                    };
                    doc.ModuleHistory.Add(historyItem);

                    //determine if serial number exists if so skip                                
                    bool canSave = !string.IsNullOrWhiteSpace(doc.Name) && !existingSerialNumbers.ContainsKey(doc.Name);
                    bool canUpdate = !string.IsNullOrWhiteSpace(doc.Name) && existingSerialNumbers.ContainsKey(doc.Name);

                    if (canSave)
                    {
                        existingSerialNumbers.Add(doc.Name, true);
                        ClientEntity existingClient = uow.ClientRepository.FindSingle(c => c.Name == clientName.Trim(), "Farms.Fields");
                        FarmEntity existingFarm = null;
                        FieldEntity existingField = null;

                        if (existingClient == null)
                        {
                            //add client/farm/field and module
                            if (!string.IsNullOrWhiteSpace(clientName))
                            {                                
                                existingClient = new ClientEntity();
                                existingClient.Farms = new List<FarmEntity>();                                
                                existingClient.Name = clientName.Trim();
                                uow.ClientRepository.CreateWithID(existingClient, clientID);                                
                            }
                            else
                            {
                                Logging.Logger.Log("INFO", string.Format("Cannot save client for new module with SN {0} - name is empty.", doc.Name));
                                return false;
                            }
                        }

                        existingFarm = uow.FarmRepository.FindSingle(f => f.Name == farmName.Trim());

                        if (existingFarm == null)
                        {
                            var canSaveFarm = !(string.IsNullOrWhiteSpace(farmName));

                            if (!canSaveFarm)
                            {
                                Logging.Logger.Log("INFO", string.Format("Cannot save farm {0} for new module with SN {1} - name empty or duplicate farm already exists for another client.", doc.FarmName, doc.Name));
                                return false;
                            }
                            else
                            {
                                //add farm/field to client                             
                                existingFarm = new FarmEntity();                                                                
                                existingFarm.Fields = new List<FieldEntity>();                                
                                existingFarm.Name = farmName.Trim();
                                existingFarm.ClientId = existingClient.Id;
                                uow.FarmRepository.CreateWithID(existingFarm, farmID);                                
                            }
                        }

                        existingField = uow.FieldRepository.FindSingle(f => f.Name.Trim() == fieldName.Trim() && f.FarmId == existingFarm.Id);

                        if (existingField == null)
                        {
                            if (!string.IsNullOrWhiteSpace(fieldName))
                            {                                
                                existingField = new FieldEntity();                                                                
                                existingField.Name = fieldName.Trim();
                                existingField.Latitude = 0.00;
                                existingField.Longitude = 0.00;
                                existingField.FarmId = existingFarm.Id;
                                uow.FieldRepository.CreateWithID(existingField, fieldID);
                            }
                            else
                            {
                                Logging.Logger.Log("INFO", string.Format("Cannot save field for new module with SN {0} - field name empty.", doc.Name));
                                return false;
                            }
                        }
                        
                        if (uow.SettingsRepository.CoordsAtFeeder(doc.Latitude, doc.Longitude))
                        {
                            doc.ModuleStatus = ModuleStatus.GINNED;
                        }
                        else if (uow.SettingsRepository.CoordsOnGinYard(doc.Latitude, doc.Longitude))
                        {
                            doc.ModuleStatus = ModuleStatus.AT_GIN;
                        }
                        else
                        {
                            doc.ModuleStatus = ModuleStatus.IN_FIELD; //change status back to field                            
                        }

                        historyItem.ModuleStatus = doc.ModuleStatus;
                        doc.FieldId = existingField.Id;
                        doc.SyncedToCloud = false; // Added 5/23
                        uow.ModuleRepository.Save(doc);
                        uow.SaveChanges();
                        return true;
                    }
                    else if (canUpdate)
                    {
                        var affectedModule = uow.ModuleRepository.FindSingle(x => x.Name == doc.Name, "ModuleHistory");
                        affectedModule.Latitude = doc.Latitude;
                        affectedModule.Longitude = doc.Longitude;
                        if (uow.SettingsRepository.CoordsAtFeeder(doc.Latitude, doc.Longitude))
                        {
                            affectedModule.ModuleStatus = ModuleStatus.GINNED;
                        }
                        else if (uow.SettingsRepository.CoordsOnGinYard(doc.Latitude, doc.Longitude))
                        {
                            affectedModule.ModuleStatus = ModuleStatus.AT_GIN;
                        }
                        else
                        {
                            affectedModule.ModuleStatus = ModuleStatus.IN_FIELD; //change status back to field                            
                        }
                        if (!string.IsNullOrWhiteSpace(doc.ImportedLoadNumber))
                            affectedModule.ImportedLoadNumber = doc.ImportedLoadNumber;

                        historyItem.ModuleStatus = affectedModule.ModuleStatus;
                        affectedModule.ModuleHistory.Add(historyItem);
                        affectedModule.SyncedToCloud = false;
                        uow.ModuleRepository.Save(affectedModule);
                        uow.SaveChanges();
                        return true;
                    }
                    else
                    {
                        Logging.Logger.Log("INFO", "Cannot save empty or duplicate serial number: " + doc.Name);
                        return false;
                    }
                }
            }
            catch(Exception exc)
            {
                Logging.Logger.Log(exc);
                return false;
            }
        }

        private bool CanReadFile(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {               
                return false;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
           
            return true;
        }

        private async Task<int> processFile(FileInfo fileInfo, int fileNumber, int totalFiles)
        {
            int errorCount = 0;
            try
            {
                int readTries = 0;
                bool canReadFile = false;

                while(readTries < 20 && !canReadFile)
                {
                    if (!CanReadFile(fileInfo))
                    {
                        canReadFile = false;
                        System.Threading.Thread.Sleep(500);  //sleep to see if file lock is released
                    }
                    else canReadFile = true;
                    readTries++;
                }

                if (!canReadFile)
                {
                    Logging.Logger.Log("ERROR", "Unable to process file " + fileInfo.FullName + ". File is in use by another process");
                    errorCount++;
                    return errorCount;
                }
                else
                {
                    Logging.Logger.Log("INFO", "Importing file " + fileInfo.FullName + ".");
                }

                var parser = new CSVFileParser(fileInfo.FullName);

                if (parser.LineCount > 1 && canReadFile)
                {   
                    int rowCount = 1;

                    //for this row need to get client, farm, field, load number if available, truck, and driver
                    clientColIndex = parser.Header.FindColumnIndex(new string[] { "Client", "Grower" });
                    farmColIndex = parser.Header.FindColumnIndex(new string[] { "Farm" });
                    fieldColIndex = parser.Header.FindColumnIndex(new string[] { "Field" });
                    loadColIndex = parser.Header.FindColumnIndex(new string[] { "Load" });
                    serialNumberColIndex = parser.Header.FindColumnIndex(new string[] { "Module SN", "SerialNumber", "Serial Number" });
                    driverColIndex = parser.Header.FindColumnIndex(new string[] { "Driver" });
                    truckColIndex = parser.Header.FindColumnIndex(new string[] { "Truck" });
                    latitudeColIndex = parser.Header.FindColumnIndex(new string[] { "Lat", "Latitude" });
                    longitudeColIndex = parser.Header.FindColumnIndex(new string[] { "Lon", "Longitude" });
                    commentsColIndex = parser.Header.FindColumnIndex(new string[] { "Comment", "Comments", "Note", "Notes" });
                    moduleIdColIndex = parser.Header.FindColumnIndex(new string[] { "ModuleID", "Module ID" });
                    clientIDColIndex = parser.Header.FindColumnIndex(new string[] { "GrowerID", "ClientID" });
                    farmIDColIndex = parser.Header.FindColumnIndex(new string[] { "FarmID" });
                    fieldIDColIndex = parser.Header.FindColumnIndex(new string[] { "FieldID" });
                    existingSerialNumbers.Clear();

                    using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                    {
                        foreach (var sn in uow.ModuleRepository.GetAllSerialNumbers())
                        {
                            existingSerialNumbers.Add(sn, true);
                        }
                    }                                       

                    int totalRows = parser.Rows.Count();
                    foreach (var row in parser.Rows)
                    {
                        updateStatus(string.Format("Processing file {0} of {1} - row {2} of {3}.", fileNumber, totalFiles, rowCount, totalRows), false);
                        if (!processRow(row, parser))
                        {
                            errorCount++;
                        }                        
                        rowCount++;
                    }                    
                }
            }
            catch (Exception exc)
            {
                errorCount++;
                Logging.Logger.Log(exc);
            }

            return errorCount;
        }        

        public LocalFileImportTask(DateTime startDate, string[] paths)
        {
            _paths = paths;
            _lastImport = startDate;                      
        }

        public async Task<int> Run()
        {
            int errorCount = 0;
            List<FileInfo> filesForImport = new List<FileInfo>();
            foreach (var path in _paths)
            {
                if (Directory.Exists(path))
                {
                    var directory = new DirectoryInfo(path);
                    updateStatus("Searching for import files in " + path, false);
                    foreach (var f in directory.GetFiles())
                    {
                        f.Refresh();
                        Logging.Logger.Log("INFO", f.Name + " Last write: " + f.LastWriteTime.ToString());
                        Logging.Logger.Log("INFO", "LAST IMPORT TIME: " + _lastImport.Value.ToString());
                        if (f.LastWriteTime > _lastImport.Value && (f.Extension.ToLower() == ".txt" || f.Extension.ToLower() == ".csv"))
                        {
                            Logging.Logger.Log("INFO", "ADDING FOR PROCESSING" + f.Name + " Last write: " + f.LastWriteTime.ToString());
                            filesForImport.Add(f);
                        }
                    }
                }
            }

            int total = filesForImport.Count();
            for (int counter = 1; counter <= total; counter++)
            {
                updateStatus(string.Format("Processing file {0} of {1}", counter, total), false);
                var errors = await processFile(filesForImport[counter - 1], counter, total);
                if (errors > 0) errorCount++;
            }
            return errorCount;
        }

        public void Dispose()
        {
            
        }
    }
}
