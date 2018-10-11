using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CottonDBMS.Data.Models.Json
{
    internal static class  DocumentTypes {
        public const string PICKUPLIST = "PICKUPLIST";
        public const string CLIENT = "CLIENT";
        public const string FARM = "FARM";
        public const string FIELD = "FIELD";
        public const string TRUCK = "TRUCK";
        public const string DRIVER = "DRIVER";
    }

    public class DocumentEntity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonIgnore]
        public bool Selected { get; set; }

        [JsonProperty(PropertyName = "downloaded")]
        public bool Downloaded { get; set; }

        [JsonProperty(PropertyName = "documenttype")]
        public string DocumentType { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; }

        [JsonProperty(PropertyName = "updated")]
        public DateTime? Updated { get; set; }
    }

    public class PickupListDocument : DocumentEntity
    {
        [JsonProperty(PropertyName = "fieldId")]
        public Guid FieldId { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "assignedTrucks")]
        public List<Guid> AssignedTrucks { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public List<Guid> DownloadedToTrucks { get; set; }

        [JsonProperty(PropertyName = "assignedModules")]
        public List<Guid> AssignedModules { get; set; }

        public PickupListDocument() : base()
        {
            DocumentType = DocumentTypes.PICKUPLIST;
        }
    }

    public class ClientDocument : DocumentEntity
    {
        [JsonProperty(PropertyName = "farms")]
        public List<Farm> Farms { get; set; }

        public ClientDocument() : base()
        {
            DocumentType = DocumentTypes.CLIENT;
        }
    }

    public class FarmDocument : DocumentEntity
    {
        [JsonProperty(PropertyName = "assignedModules")]
        public List<Field> Fields { get; set; }

        public FarmDocument() : base()
        {
            DocumentType = DocumentTypes.FARM;
        }
    }

    public class FieldDocument : DocumentEntity
    {
        public FieldDocument() : base()
        {
            DocumentType = DocumentTypes.FIELD;
        }
    }

    public class TruckDocument : DocumentEntity
    {
        public TruckDocument() : base()
        {
            DocumentType = DocumentTypes.TRUCK;
        }
    }

    public class DriverDocument : DocumentEntity
    {
        [JsonProperty(PropertyName = "firstname")]
        public string Firstname { get; set; }

        [JsonProperty(PropertyName = "lastname")]
        public string Lastname { get; set; }

        public DriverDocument() : base()
        {
            DocumentType = DocumentTypes.DRIVER;
        }
    }    
}
