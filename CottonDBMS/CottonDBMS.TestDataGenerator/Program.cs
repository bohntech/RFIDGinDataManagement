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
        
        static Random rand = new Random(DateTime.Now.Millisecond);

        private static void InitializeDataSources()
        {
          
        }

        private static async Task processClients(ClientEntity client, List<ModuleEntity> modulesToAdd)
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
                    int stage = rand.Next(1, 5);

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
                            ModuleStatus = ModuleStatus.GINNED
                        };

                        module.ModuleStatus = ModuleStatus.GINNED;
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

        static void Main(string[] args)
        {
            Task.Run(async () => await MainAsync(args)).Wait();
        }

        static async Task MainAsync(string[] args)
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

            int numClients = 1;
            int numFarmsPerClient = 1;
            int numFieldsPerFarm = 1;
            int numModulesPerField = 1;

            

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
                            module.LoadNumber = "2017M" + loadNumber.ToString().PadLeft(7, '0');                            
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

                await processClients(client, modulesToAdd);
                clientsToAdd.Clear();
                modulesToAdd.Clear();
            }           

            Console.WriteLine("Test data generation complete.");
            Console.ReadLine();
        }
    }
}

