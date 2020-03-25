using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using CottonDBMS.Cloud;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using CottonDBMS.Data.EF;

namespace CottonDBMS.TestDataGenerator
{
    class Program
    {
        static double feederLat = 33.693064;
        static double feederLong = -102.070150;

        static double yardLat = 33.693537;
        static double yardLong = -102.075632;

        static double fieldStartLat = 33.794750; //33.794322
        static double fieldStartLong = -102.078223; //-102.077537

        //static double nextfieldLatIncrement = 0.15900;
        //static double nextfieldLongIncrement = 0.15900;

        static double fieldLatIncrement = .00010;
        static double fieldLongIncrement = .0010;

        static int totalFields = 0;

        static int clientCount = 0;

        static Random rand = new Random(55);

        private static void InitializeDataSources()
        {

        }

        private static void createPBIData()
        {
            FieldEntity field = null;
            ModuleEntity module = null;

            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                #region Create Modules and Gin Loads
                var client = new ClientEntity();
                client.Name = "PBI TEST CLIENT";
                client.Farms = new List<FarmEntity>();

                var farm = new FarmEntity();
                farm.Id = Guid.NewGuid().ToString();
                farm.Name = "PBI TEST FARM";
                farm.Fields = new List<FieldEntity>();

                field = new FieldEntity();
                field.Id = Guid.NewGuid().ToString();
                field.Name = "PBI TEST FIELD";
                field.Modules = new List<ModuleEntity>();
                field.Latitude = 0.00;
                field.Longitude = 0.00;
                field.Created = DateTime.UtcNow;
                farm.Fields.Add(field);

                client.Farms.Add(farm);
                uow.ClientRepository.Save(client);
                uow.SaveChanges();
            }

            //add modules from CSV file
            string fileName = "C:\\Users\\mbohn\\Documents\\PBI_TestData.csv";
            string[] moduleContents = System.IO.File.ReadAllLines(fileName);

            List<GinLoadEntity> ginLoadEntities = new List<GinLoadEntity>();
            int bridgeLoadNumber = 0;
            for (int moduleCount = 1; moduleCount < moduleContents.Length; moduleCount++)
            {
                var fields = moduleContents[moduleCount].Split(',');
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    module = new ModuleEntity();
                    module.Id = Guid.NewGuid().ToString();
                    module.Name = fields[0];
                    module.FieldId = field.Id;
                    module.TruckID = "";
                    module.Driver = "";
                    module.LoadNumber = fields[3];
                    module.HIDDiameter = Decimal.Parse(fields[1]) * 2.54M;
                    module.HIDModuleWeight = Decimal.Parse(fields[2]) * 0.453592M;

                    module.Latitude = 0.00;
                    module.Longitude = 0.00;
                    module.ModuleStatus = ModuleStatus.IN_FIELD;
                    module.Created = DateTime.UtcNow;
                    module.ModuleHistory = new List<ModuleHistoryEntity>();
                    var historyItem = new ModuleHistoryEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Created = DateTime.UtcNow,
                        Driver = module.Driver,
                        TruckID = module.TruckID,
                        Latitude = module.Latitude,
                        Longitude = module.Longitude,
                        ModuleEventType = ModuleEventType.IMPORTED_FROM_FILE
                    };
                    module.FieldId = field.Id;
                    module.ModuleHistory.Add(historyItem);
                    uow.ModuleRepository.Add(module);
                    uow.SaveChanges();

                    if (!ginLoadEntities.Any(g => g.GinTagLoadNumber == module.LoadNumber))
                    {
                        
                        //create gin load
                        var affectedGinLoad = new GinLoadEntity();
                        ginLoadEntities.Add(affectedGinLoad);
                        affectedGinLoad.Created = DateTime.UtcNow;
                        affectedGinLoad.SyncedToCloud = false;

                        affectedGinLoad.Name = module.LoadNumber;
                        affectedGinLoad.GinTagLoadNumber = module.LoadNumber;
                        affectedGinLoad.ScaleBridgeLoadNumber = bridgeLoadNumber++;
                        affectedGinLoad.NetWeight = Convert.ToDecimal(fields[4]);
                        affectedGinLoad.GrossWeight = affectedGinLoad.NetWeight + 8200;
                        affectedGinLoad.SplitWeight1 = affectedGinLoad.NetWeight;
                        affectedGinLoad.SplitWeight2 = 0.00M;

                        affectedGinLoad.ScaleBridgeId = "BRIDGE1";
                        affectedGinLoad.ScaleBridgeLoadNumber = ginLoadEntities.Count();
                        affectedGinLoad.Source = InputSource.GIN;
                        affectedGinLoad.SyncedToCloud = false;
                        affectedGinLoad.TruckID = "TRUCK1";
                        affectedGinLoad.YardLocation = "SOMEWHERE";
                        affectedGinLoad.SubmittedBy = "TEST HARNESS";

                        //add client/farm/field information
                        affectedGinLoad.FieldId = field.Id;
                        uow.GinLoadRepository.Save(affectedGinLoad);
                        uow.SaveChanges();

                        module.GinTagLoadNumber = affectedGinLoad.GinTagLoadNumber;
                        module.GinLoadId = affectedGinLoad.Id;
                        
                    }
                    else
                    {
                        var load = ginLoadEntities.Single(g => g.GinTagLoadNumber == module.LoadNumber);
                        module.GinTagLoadNumber = load.GinTagLoadNumber;
                        module.GinLoadId = load.Id;
                    }

                    //create feeder scan of module
                    module.ModuleStatus = ModuleStatus.ON_FEEDER;
                    module.LastBridgeId = "BRIDGE2";
                   // uow.ModuleRepository.Save(module);
                    //uow.SaveChanges();
                    Console.WriteLine("CREATING FEEDER SCAN FOR MODULE : " + module.Name);
                    ModuleHistoryEntity feederHistoryItem = new ModuleHistoryEntity();
                    feederHistoryItem.Id = Guid.NewGuid().ToString();
                    feederHistoryItem.Created = DateTime.Parse(fields[5] + " " + fields[6]).ToUniversalTime();
                    feederHistoryItem.Created = feederHistoryItem.Created.AddSeconds(int.Parse(fields[7]));
                    feederHistoryItem.Driver = "";
                    feederHistoryItem.TruckID = "";
                    feederHistoryItem.BridgeId = "BRIDGE2";
                    feederHistoryItem.ModuleId = module.Id;
                    feederHistoryItem.Latitude = module.Latitude;
                    feederHistoryItem.Longitude = module.Longitude;
                    feederHistoryItem.GinTagLoadNumber = module.GinTagLoadNumber;

                    if (!string.IsNullOrEmpty(module.BridgeLoadNumber))
                        feederHistoryItem.BridgeLoadNumber = bridgeLoadNumber;
                    else
                        feederHistoryItem.BridgeLoadNumber = null;

                    module.ModuleHistory.Add(feederHistoryItem);
                    feederHistoryItem.ModuleStatus = ModuleStatus.ON_FEEDER;
                    feederHistoryItem.ModuleStatus = module.ModuleStatus;
                    feederHistoryItem.ModuleEventType = ModuleEventType.BRIDGE_SCAN;
                    module.SyncedToCloud = false;
                    uow.ModuleRepository.Save(module);
                    uow.SaveChanges();
                }
            }

            #endregion

            #region Create PBIs
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                string pbiFileName = "C:\\Users\\mbohn\\Documents\\PBI_Test_Bales.csv";
                string[] pbiLines = System.IO.File.ReadAllLines(pbiFileName);                
                for (int lineCount=1; lineCount < pbiLines.Length; lineCount++)
                {
                    string line = pbiLines[lineCount];
                    var pbiFields = line.Split(',');
                    var affectedBale = new BaleEntity();
                    affectedBale.Id = Guid.NewGuid().ToString();
                    affectedBale.Name = pbiFields[0];
                    affectedBale.PbiNumber = pbiFields[0];
                    affectedBale.Created = DateTime.Parse(pbiFields[1] + " " + pbiFields[2]).ToUniversalTime();
                    affectedBale.Created = affectedBale.Created.AddMilliseconds(lineCount);
                    affectedBale.SyncedToCloud = false;
                    affectedBale.NetWeight = decimal.Parse(pbiFields[3]);
                    affectedBale.WeightFromScale = affectedBale.NetWeight + 3.0M;
                    affectedBale.TareWeight = 3.0M;
                    affectedBale.ScanNumber = lineCount;
                    affectedBale.SyncedToCloud = false;
                    affectedBale.NetWeight = decimal.Parse(pbiFields[3]);
                    Console.WriteLine("CREATING PBI: " + affectedBale.PbiNumber + " " + affectedBale.Created.ToString("MM/dd/yyyy HH:mm:ss"));
                    uow.BalesRepository.Add(affectedBale);
                }
                uow.SaveChanges();
            }
            #endregion
        }
    

        private static void processClients(ClientEntity client, List<ModuleEntity> modulesToAdd)
        {

            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                Console.WriteLine("Creating client: " + client.Name);                
                clientCount++;

                //generate history records for modules          
                int yardCol = 1;
                int yardRow = 1;
                foreach (var module in modulesToAdd)
                {
                    int stage = 2; // rand.Next(1, 5);

                    if (stage >= 2)
                    {
                        ModuleHistoryEntity pickupItem = new ModuleHistoryEntity
                        {
                            Id = Guid.NewGuid().ToString(),
                            Created = module.Created.AddDays(2),
                            Driver = module.Driver,
                            TruckID = module.TruckID,
                            Latitude = module.Latitude,
                            Longitude = module.Longitude,
                            ModuleEventType = ModuleEventType.LOADED,
                            ModuleStatus = ModuleStatus.PICKED_UP

                        };
                        module.ModuleStatus = ModuleStatus.PICKED_UP;
                        module.ModuleHistory.Add(pickupItem);
                    }

                    if (stage >= 3)
                    {
                        ModuleHistoryEntity atGinItem = new ModuleHistoryEntity
                        {
                            Id = Guid.NewGuid().ToString(),
                            Created = module.Created.AddDays(4),
                            Driver = module.Driver,
                            TruckID = module.TruckID,
                            Latitude = yardLat + fieldLatIncrement * yardRow,
                            Longitude = yardLong + fieldLongIncrement * yardCol,
                            ModuleEventType = ModuleEventType.UNLOADED,
                            ModuleStatus = ModuleStatus.AT_GIN
                        };

                        yardCol++;
                        if (yardCol > 50)
                        {
                            yardCol = 1;
                            yardRow++;
                        }

                        module.ModuleStatus = ModuleStatus.AT_GIN;
                        module.ModuleHistory.Add(atGinItem);
                    }

                    if (stage >= 4)
                    {
                        ModuleHistoryEntity loadedAtGinYard = new ModuleHistoryEntity
                        {
                            Id = Guid.NewGuid().ToString(),
                            Created = module.Created.AddDays(6),
                            Driver = module.Driver,
                            TruckID = module.TruckID,
                            Latitude = yardLat + fieldLatIncrement * yardRow,
                            Longitude = yardLong + fieldLongIncrement * yardCol,
                            ModuleEventType = ModuleEventType.LOADED,
                            ModuleStatus = ModuleStatus.AT_GIN
                        };

                        yardCol++;
                        if (yardCol > 25)
                        {
                            yardCol = 1;
                            yardRow++;
                        }

                        ModuleHistoryEntity unloadedAtFeeder = new ModuleHistoryEntity
                        {
                            Id = Guid.NewGuid().ToString(),
                            Created = module.Created.AddDays(6).AddMinutes(10),
                            Driver = module.Driver,
                            TruckID = module.TruckID,
                            Latitude = feederLat,
                            Longitude = feederLong,
                            ModuleEventType = ModuleEventType.UNLOADED,
                            ModuleStatus = ModuleStatus.ON_FEEDER
                        };

                        module.ModuleStatus = ModuleStatus.ON_FEEDER;
                        module.ModuleHistory.Add(loadedAtGinYard);
                        module.ModuleHistory.Add(unloadedAtFeeder);
                    }
                }

                Console.WriteLine("Creating test modules...");
                //await CloudDataProvider.ModulesRepository.BulkCreateAsync(modulesToAdd, 250);
                uow.ClientRepository.Save(client);
                uow.SaveChanges();
            }
        }

        private static void createGinLoads()
        {
            int currentGinTagNumber = 5000;
            int bridgeLoad = 0;
            System.Random rand = new Random(55);
            using (IUnitOfWork uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                var fields = uow.FieldRepository.GetAll().OrderBy(x => x.Created).ToList();

                foreach (var field in fields)
                {
                    Console.WriteLine("CREATING LOADS FOR FIELD: " + field.Name);
                    var modules = uow.ModuleRepository.FindMatching(m => m.FieldId == field.Id, new string[] { "ModuleHistory" }).ToArray();
                    
                    int loadModuleCount = 0;
                    GinLoadEntity affectedGinLoad = null;
                    for (var i = 0; i < modules.Count(); i++)
                    {
                        if (loadModuleCount == 0 || loadModuleCount == 4)
                        {
                            loadModuleCount = 0;
                            bridgeLoad++;
                            currentGinTagNumber++;
                            Console.WriteLine("CREATING LOAD: " + currentGinTagNumber.ToString());
                            //break modules up into loads of 4                                      
                            affectedGinLoad = new GinLoadEntity();
                            affectedGinLoad.Created = DateTime.UtcNow;
                            affectedGinLoad.SyncedToCloud = false;

                            affectedGinLoad.Name = currentGinTagNumber.ToString();
                            affectedGinLoad.GinTagLoadNumber = currentGinTagNumber.ToString();

                            affectedGinLoad.NetWeight = Convert.ToDecimal(rand.Next(18000, 20300));
                            affectedGinLoad.GrossWeight = affectedGinLoad.NetWeight + 8200;
                            affectedGinLoad.SplitWeight1 = affectedGinLoad.NetWeight;
                            affectedGinLoad.SplitWeight2 = 0.00M;

                            affectedGinLoad.ScaleBridgeId = "BRIDGE1";
                            affectedGinLoad.ScaleBridgeLoadNumber = bridgeLoad;
                            affectedGinLoad.Source = InputSource.GIN;
                            affectedGinLoad.SyncedToCloud = false;

                            affectedGinLoad.TruckID = "TRUCK1";

                            affectedGinLoad.YardLocation = "SOMEWHERE";
                            affectedGinLoad.SubmittedBy = "TEST HARNESS";

                            //add client/farm/field information
                            affectedGinLoad.FieldId = field.Id;
                            uow.GinLoadRepository.Save(affectedGinLoad);
                            uow.SaveChanges();
                        }


                        var module = modules[i];
                        module.Updated = DateTime.UtcNow;

                        //update module with scan information and link to load
                        module.TruckID = affectedGinLoad.TruckID;
                        module.LastBridgeId = "BRIDGE1";
                        module.GinLoadId = affectedGinLoad.Id;

                        if (string.IsNullOrEmpty(module.FirstBridgeId))
                            module.FirstBridgeId = "BRIDGE1";

                        module.GinTagLoadNumber = affectedGinLoad.GinTagLoadNumber;
                        module.ClassingModuleId = "";
                        module.ModuleStatus = ModuleStatus.AT_GIN;

                        ModuleHistoryEntity historyItem = new ModuleHistoryEntity();
                        historyItem.Id = Guid.NewGuid().ToString();
                        historyItem.Created = DateTime.UtcNow;//moduleScan.ScanTime.HasValue ? moduleScan.ScanTime.Value : DateTime.UtcNow;
                        historyItem.Driver = "";
                        historyItem.TruckID = "TRUCK1";
                        historyItem.BridgeId = "BRIDGE1";
                        historyItem.BridgeLoadNumber = bridgeLoad;
                        historyItem.GinTagLoadNumber = affectedGinLoad.GinTagLoadNumber;
                        historyItem.Name = affectedGinLoad.GinTagLoadNumber;
                        historyItem.ModuleStatus = ModuleStatus.AT_GIN;
                        historyItem.Latitude = module.Latitude;
                        historyItem.Longitude = module.Longitude;
                        historyItem.ModuleEventType = ModuleEventType.BRIDGE_SCAN;
                        historyItem.SyncedToCloud = false;
                        historyItem.ModuleId = module.Id;
                        module.ModuleHistory.Add(historyItem);
                        uow.ModuleRepository.Save(module);
                        // uow.SaveChanges();
                       
                        loadModuleCount++;
                    }
                    uow.SaveChanges();
                }
            }
        }

        private static void createFeederScans()
        {
            //create feeder scan for each load
            DateTime currentModuleScan = new DateTime(2019, 9, 15, 8, 0, 0);
            currentModuleScan = currentModuleScan.ToUniversalTime();


            int feederToBaleMaxMinutes = 10;
            int feedetToBaleMinMunutes = 4;
            int minBaleWeight = 475;
            int maxBaleWeight = 510;
            int minModuleTurnout = 39;
            int maxModuleTurnout = 45;
            int lastBaleNumber = 0;

            try
            {
                

                //get each load in order
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    var loads = uow.GinLoadRepository.GetAll(new string[] { "Modules.ModuleHistory" }).OrderBy(x => x.ScaleBridgeLoadNumber);
                    foreach (var l in loads)
                    {
                        Console.WriteLine("CREATING SCANS FOR LOAD: " + l.GinTagLoadNumber);
                        foreach (var affectedModule in l.Modules)
                        {
                            affectedModule.ModuleStatus = ModuleStatus.ON_FEEDER;
                            affectedModule.LastBridgeId = "BRIDGE2";
                            Console.WriteLine("CURRENT FEEDER SCAN TIME: " + currentModuleScan.ToString("MM/dd/yyyy HH:mm:ss"));
                            ModuleHistoryEntity historyItem = new ModuleHistoryEntity();
                            historyItem.Id = Guid.NewGuid().ToString();
                            historyItem.Created = currentModuleScan;
                            historyItem.Driver = "";
                            historyItem.TruckID = "";
                            historyItem.BridgeId = "BRIDGE2";
                            historyItem.ModuleId = affectedModule.Id;
                            historyItem.Latitude = affectedModule.Latitude;
                            historyItem.Longitude = affectedModule.Longitude;
                            historyItem.GinTagLoadNumber = affectedModule.GinTagLoadNumber;

                            if (!string.IsNullOrEmpty(affectedModule.BridgeLoadNumber))
                                historyItem.BridgeLoadNumber = int.Parse(affectedModule.BridgeLoadNumber);
                            else
                                historyItem.BridgeLoadNumber = null;

                            affectedModule.ModuleHistory.Add(historyItem);
                            historyItem.ModuleStatus = ModuleStatus.ON_FEEDER;
                            historyItem.ModuleStatus = affectedModule.ModuleStatus;
                            historyItem.ModuleEventType = ModuleEventType.BRIDGE_SCAN;
                            affectedModule.SyncedToCloud = false;
                            uow.ModuleRepository.Save(affectedModule);
                            uow.SaveChanges();

                            //create PBI scans for module
                            decimal accumulatedWeight = 0.00M;
                            decimal currentModuleTurnout = Convert.ToDecimal(rand.Next(minModuleTurnout, maxModuleTurnout)) / 100.00M;

                            decimal selectedWeight = 0.00M;

                            if (affectedModule.HIDModuleWeightLBS.HasValue) selectedWeight = affectedModule.HIDModuleWeightLBS.Value;
                            else if (affectedModule.DiameterApproximatedWeight.HasValue) selectedWeight = affectedModule.DiameterApproximatedWeight.Value;
                            else selectedWeight = affectedModule.LoadAvgModuleWeight.Value;

                            decimal currentModuleLintWeight = selectedWeight * currentModuleTurnout;
                            DateTime currentPBIScanTime = currentModuleScan.AddMinutes(rand.Next(feedetToBaleMinMunutes, feederToBaleMaxMinutes));
                            currentPBIScanTime = currentPBIScanTime.AddSeconds(rand.Next(1, 30));
                            while (accumulatedWeight < currentModuleLintWeight)
                            {
                                lastBaleNumber++;
                                var affectedBale = new BaleEntity();
                                affectedBale.Id = Guid.NewGuid().ToString();
                                affectedBale.Name = int.Parse(affectedModule.Name).ToString().PadLeft(5, '0') + lastBaleNumber.ToString().PadLeft(6, '0').ToString();
                                affectedBale.PbiNumber = affectedBale.Name;
                                affectedBale.Created = currentPBIScanTime;
                                affectedBale.SyncedToCloud = false;

                                affectedBale.WeightFromScale = rand.Next(minBaleWeight, maxBaleWeight);
                                affectedBale.TareWeight = 3.0M;
                                affectedBale.ScanNumber = lastBaleNumber;
                                affectedBale.SyncedToCloud = false;
                                affectedBale.NetWeight = affectedBale.WeightFromScale - affectedBale.TareWeight;

                                accumulatedWeight += affectedBale.NetWeight;

                                if (accumulatedWeight > currentModuleLintWeight) //only apply weight to equal lint weight
                                {
                                    affectedBale.NetWeight = affectedBale.NetWeight - (accumulatedWeight - currentModuleLintWeight);

                                    if (rand.Next(10) > 5)
                                        affectedBale.NetWeight += rand.Next(1, 3);
                                    else
                                        affectedBale.NetWeight -= rand.Next(1, 3);

                                    affectedBale.WeightFromScale = affectedBale.NetWeight + 3.0M;
                                }
                                Console.WriteLine("CREATING PBI: " + affectedBale.PbiNumber + " " + affectedBale.Created.ToString("MM/dd/yyyy HH:mm:ss"));
                                uow.BalesRepository.Add(affectedBale);
                               
                                currentPBIScanTime = currentPBIScanTime.AddMinutes(rand.Next(1, 3));
                            }
                            uow.SaveChanges();
                            //set next module scan half way between last module scan and current pbi scan
                            var timeSpan = currentPBIScanTime.Subtract(currentModuleScan);
                            currentModuleScan = currentPBIScanTime.AddSeconds(feedetToBaleMinMunutes * 60 / 2 * -1);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        static void Main(string[] args)
        {
            MainTask(args);
        }

        static void MainTask(string[] args)
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            dir = dir.TrimEnd('\\') + "\\" + FolderConstants.ROOT_APP_DATA_FOLDER;

            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }

            CottonDBMS.Logging.Logger.SetLogPath(Environment.CurrentDirectory);

            AppDomain.CurrentDomain.SetData("DataDirectory", dir.TrimEnd('\\'));

            InitializeDataSources();

            Console.WriteLine("Would you like to generate sample data for PBI algorithm? (Y/N)");
            string response = Console.ReadLine();
            if (response.ToUpper() == "Y")
            {
                createPBIData();
                Console.WriteLine("PBI Data generated.  Press enter to exit.");
                Console.ReadLine();
                return;
            }

            string clientStartNumber = "";
            Console.WriteLine("Enter client start number: ");
            clientStartNumber = Console.ReadLine();
            int clientStartNum = 0;
            int.TryParse(clientStartNumber, out clientStartNum);

            Console.WriteLine("Enter number of clients to generate: ");
            string numClientsStr = Console.ReadLine();

            Console.WriteLine("Enter number of farms per client: ");
            string numFarmsPerClientStr = Console.ReadLine();

            Console.WriteLine("Enter number of fields per client: ");
            string numFieldsPerFarmStr = Console.ReadLine();

            Console.WriteLine("Enter number of modules per field: ");
            string numModulesPerFieldStr = Console.ReadLine();

            //Console.WriteLine("Enter number of modules per load: ");
            //string numModulesPerLoad = Console.ReadLine();

            int numClients = 1;
            int numFarmsPerClient = 1;
            int numFieldsPerFarm = 1;
            int numModulesPerField = 1;
            //int numModulesPerLoad = 4;
            

            double clientFieldLat = fieldStartLat + (rand.NextDouble() * 1);
            double clientFieldLong = fieldStartLong + (rand.NextDouble() * 1);

            List<ClientEntity> clientsToAdd = new List<ClientEntity>();
            List<ModuleEntity> modulesToAdd = new List<ModuleEntity>();

            int.TryParse(numClientsStr, out numClients);
            int.TryParse(numFarmsPerClientStr, out numFarmsPerClient);
            int.TryParse(numFieldsPerFarmStr, out numFieldsPerFarm);
            int.TryParse(numModulesPerFieldStr, out numModulesPerField);

            string[] drivers = { "John Doe", "Jerry Jones", "E. Smith" };
            string[] trucks = { "Truck 1", "Truck 2", "Truck 3", "Truck 4" };

            
            int totalModules = 1;
            int loadNumber = 1;

            for(int clientIndex = 0; clientIndex < numClients; clientIndex++)
            {
                var client = new ClientEntity();
                
                client.Name = string.Format("Client {0}", (clientIndex + clientStartNum));                               
                client.Farms = new List<FarmEntity>();
                clientsToAdd.Add(client);

                for (int farmCount = 1; farmCount <= numFarmsPerClient; farmCount++)
                {
                    var farm = new FarmEntity();
                    farm.Id = Guid.NewGuid().ToString();
                    farm.Name = string.Format("{0} - Farm {1}", client.Name, farmCount);                                        
                    farm.Fields = new List<FieldEntity>();
                    for (int fieldCount = 1; fieldCount <= numFieldsPerFarm; fieldCount++)
                    {                        
                        DateTime addDate = DateTime.UtcNow.AddDays(rand.Next(30) * -1);
                        var field = new FieldEntity();
                        field.Id = Guid.NewGuid().ToString();
                        field.Name = string.Format("{0} - Field {1}", farm.Name, fieldCount);
                        field.Modules = new List<ModuleEntity>();
                        field.Latitude = clientFieldLat;
                        field.Longitude = clientFieldLong;
                        field.Created = addDate;                        
                        farm.Fields.Add(field);

                        totalFields++;

                        int fieldCol = 1;
                        int fieldRow = 1;

                        clientFieldLat = fieldStartLat + (rand.NextDouble());
                        clientFieldLong = fieldStartLong + (rand.NextDouble());

                        for (int moduleCount = 1; moduleCount <= numModulesPerField; moduleCount++)
                        {
                            ModuleEntity module = new ModuleEntity();                            
                            module.Id = Guid.NewGuid().ToString();
                            module.Name = totalModules.ToString().PadLeft(7, '0');
                            module.FieldId = field.Id;                          
                            module.TruckID = trucks[rand.Next(trucks.Length - 1)];
                            module.Driver = drivers[rand.Next(drivers.Length - 1)];
                            module.LoadNumber = "2019M" + loadNumber.ToString().PadLeft(7, '0');

                            int weightSelector = rand.Next(1, 4);

                            if (weightSelector == 1) //use module weight
                            {
                                module.HIDDiameter = rand.Next(225, 240);
                                module.HIDModuleWeight = rand.Next(2200, 2650);
                            }
                            else if (weightSelector == 2) //use diameter
                            {
                                module.HIDDiameter = rand.Next(225, 240);
                                module.HIDModuleWeight = null;
                            }
                            else
                            {
                                module.HIDDiameter = null;
                                module.HIDModuleWeight = null;
                            }
                           
                            totalModules++;

                            if (totalModules % 4 == 0)
                            {
                                loadNumber++;
                            }
                            
                            module.Latitude = clientFieldLat + (fieldRow * fieldLatIncrement);
                            module.Longitude = clientFieldLong + (fieldCol*fieldLongIncrement);
                            module.ModuleStatus = ModuleStatus.IN_FIELD;
                            module.Created = addDate;
                            module.ModuleHistory = new List<ModuleHistoryEntity>();
                            var historyItem = new ModuleHistoryEntity
                            {
                                Id = Guid.NewGuid().ToString(),
                                Created = addDate,
                                Driver = module.Driver,
                                TruckID = module.TruckID,
                                Latitude = module.Latitude,
                                Longitude = module.Longitude,
                                ModuleEventType = ModuleEventType.IMPORTED_FROM_FILE
                            };
                            field.Modules.Add(module);
                            module.ModuleHistory.Add(historyItem);
                            modulesToAdd.Add(module);
                            fieldCol++;
                            if (fieldCol >= 25)
                            {
                                fieldCol = 1;
                                fieldRow++;
                            }
                        }
                    }
                    client.Farms.Add(farm);                    
                }

                processClients(client, modulesToAdd);

                clientsToAdd.Clear();
                modulesToAdd.Clear();
            
            }

            createGinLoads();
            createFeederScans();

            Console.WriteLine("Test data generation complete.");
            Console.ReadLine();
        }
    }
}

