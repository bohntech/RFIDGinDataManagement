using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using CottonDBMS.DataModels.Helpers;

namespace CottonDBMS.DataModels
{
    public class BaleScanEntity : BaseEntity
    {
        public string PbiNumber { get; set; }
        public int ScanNumber { get; set; }
        public decimal ScaleWeight { get; set; }
        public bool Processed { get; set; }
        public bool OutOfSequence { get; set; }
        public decimal TareWeight { get; set; }
        public decimal NetLintBaleWeight { get { return ScaleWeight - TareWeight; } }

        public void CopyTo(BaleScanEntity target)
        {
            target.Id = Id;
            target.PbiNumber = PbiNumber;
            target.ScanNumber = ScanNumber;
            target.ScaleWeight = ScaleWeight;
            target.TareWeight = TareWeight;
            target.Processed = Processed;
            target.Created = Created;
            target.EntityType = EntityType;
            target.Name = Name;
            target.SelfLink = SelfLink;
            target.Source = Source;
            target.SyncedToCloud = SyncedToCloud;            
            target.Updated = Updated;
            target.OutOfSequence = OutOfSequence;            
        }

        public BaleScanEntity() : base()
        {
            EntityType = EntityType.BALE_SCAN;
            SyncedToCloud = false;
            Source = InputSource.TRUCK;
        }

        public override string ToString()
        {
            string sequenceStr = "";
            if (OutOfSequence) sequenceStr = " - OUT OF SEQUENCE ";
            return PbiNumber.ToString().Trim().PadLeft(25, ' ') + (ScaleWeight.ToString("0.00") + "LBS").PadLeft(25, ' ') + Created.ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss tt").PadLeft(25, ' ') + sequenceStr;
        }
    }
}
