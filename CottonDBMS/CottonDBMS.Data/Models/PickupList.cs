using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.Data.Models
{
    public class PickupList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }      
        public Field Field { get; set; }
        public Guid FieldId { get; set; }
        
        public List<Truck> AssignedTrucks { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public bool IsModified { get; set; }
        public bool IsNew { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public bool IsModified { get; set; }
        public bool IsNew { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public class Farm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Client Client { get; set; }
        public Guid ClientId { get; set; }

        public bool IsModified { get; set; }
        public bool IsNew { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public class Field
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Farm Farm { get; set; }
        public Guid FarmId { get; set; }

        public bool IsModified { get; set; }
        public bool IsNew { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public enum ModuleSource { FILE_IMPORT, EMAIL_IMPORT, MANUAL_ENTRY};
    public enum ModuleStatus { IN_FIELD, PICKED_UP, AT_GIN, GINNED};
    public enum ModuleEventType {FIELD_SCAN, LOADED, UNLOADED, MANUAL_EDIT};
    
    public class Module
    {
        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        public Guid FieldId { get; set; }
        public Field Field { get; set; }

        public Guid PickupListId { get; set; }
        public PickupList PickupList {get; set;}

        public int LoadNumber { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ModuleSource Source { get; set; }
        public ModuleStatus Status { get; set; }

        public bool IsModified { get; set; }
        public bool IsNew { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public class ModuleEvent
    {
        public Guid Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ModuleEventType EventType { get; set; }

        public bool IsModified { get; set; }
        public bool IsNew { get; set; }

        public DateTime Timestamp { get; set; }
    }

    public class Truck
    {
        public Guid Id { get; set; }
        public string TruckName { get; set; }

        public bool IsModified { get; set; }
        public bool IsNew { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public class Driver
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public bool IsModified { get; set; }
        public bool IsNew { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
