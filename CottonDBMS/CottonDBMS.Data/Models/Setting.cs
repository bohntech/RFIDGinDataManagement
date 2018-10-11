using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.Data.Models
{
    public static class TruckClientSettingKeys
    {
        public const string A1_XMIT_POWER = "A1_XMIT_POWER";
        public const string A2_XMIT_POWER = "A2_XMIT_POWER";
        public const string A3_XMIT_POWER = "A3_XMIT_POWER";
        public const string A4_XMIT_POWER = "A4_XMIT_POWER";

        public const string A1_RECV_SENSITIVITY = "A1_RECV_SENSITINITY";
        public const string A2_RECV_SENSITIVITY = "A2_RECV_SENSITINITY";
        public const string A3_RECV_SENSITIVITY = "A3_RECV_SENSITINITY";
        public const string A4_RECV_SENSITIVITY = "A4_RECV_SENSITINITY";

        public const string GPS_COM_PORT = "GPS_COM_PORT";

        public const string RFID_READ_DELAY = "RFID_READ_DELAY";
        public const string TRUCK_ID = "TRUCK_ID";
        public const string DRIVER_ID = "DRIVER_ID";
        public const string DOCUMENT_DB_KEY = "DOCUMENT_DB_KEY";
        public const string DATA_SYNC_INTERVAL = "DATA_SYNC_INTERVAL";
        public const string ADMIN_PASSWORD = "ADMIN_PASSWORD";
    }

    public static class GinAppSettingKeys
    {
        public const string IMPORT_FOLDER = "IMPORT_FOLDER";
        public const string ARCHIVE_FOLDER = "ARCHIVE_FOLDER";

        public const string IMAP_HOSTNAME = "IMAP_HOSTNAME";
        public const string IMAP_PORT = "IMAP_PORT";
        public const string IMAP_USERNAME = "IMAP_USERNAME";
        public const string IMAP_PASSWORD = "IMAP_PASSWORD";

        public const string GIN_YARD_NW_CORNER_NORTH = "GIN_YARD_NW_CORNER_NORTH";
        public const string GIN_YARD_NW_CORNER_WEST = "GIN_YARD_NW_CORNER_WEST";

        public const string GIN_YARD_SE_CORNER_NORTH = "GIN_YARD_SE_CORNER_NORTH";
        public const string GIN_YARD_SE_CORNER_WEST = "GIN_YARD_SE_CORNER_WEST";

        public const string GIN_FEEDER_NORTH = "GIN_FEEDER_NORTH";
        public const string GIN_FEEDER_WEST = "GIN_FEEDER_WEST";

        public const string GIN_FEEDER_DETECTION_RADIUS = "GIN_FEEDER_DETECTION_RADIUS";
        public const string GOOGLE_MAPS_API_KEY = "GOOGLE_MAPS_API_KEY";

        public const string IMPORT_INTERVAL = "IMPORT_INTERVAL";

        public const string AZURE_DOCUMENTDB_ENDPOINT = "AZURE_DOCUMENTDB_ENDPOINT";
        public const string AZURE_DOCUMENTDB_KEY = "AZURE_DOCUMENTDB_KEY";
    }

    public class Setting
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
