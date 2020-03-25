//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.DataModels
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
        public const string DOCUMENTDB_ENDPOINT = "TRUCK_DOCUMENTDB_ENDPOINT";
        public const string DATA_SYNC_INTERVAL = "DATA_SYNC_INTERVAL";
        public const string ADMIN_PASSWORD = "ADMIN_PASSWORD";
        public const string MODULES_ON_TRUCK = "MODULES_ON_TRUCK";

        public const string SYNC_RUNNING = "SYNC_RUNNING";

        public const string GPS_OFFSET_FEET = "GPS_OFFSET_FEET";
    }

    public static class BridgeSettingKeys
    {
        public const string A1_XMIT_POWER = "BRIDGE_A1_XMIT_POWER";
        public const string A2_XMIT_POWER = "BRIDGE_A2_XMIT_POWER";
        public const string A3_XMIT_POWER = "BRIDGE_A3_XMIT_POWER";
        public const string A4_XMIT_POWER = "BRIDGE_A4_XMIT_POWER";

        public const string A1_RECV_SENSITIVITY = "BRIDGE_A1_RECV_SENSITINITY";
        public const string A2_RECV_SENSITIVITY = "BRIDGE_A2_RECV_SENSITINITY";
        public const string A3_RECV_SENSITIVITY = "BRIDGE_A3_RECV_SENSITINITY";
        public const string A4_RECV_SENSITIVITY = "BRIDGE_A4_RECV_SENSITINITY";

        public const string TARE_WEIGHT = "BRIDGE_TARE_WEIGHT";
        public const string SCALE_COM_PORT = "BRIDGE_SCALE_COM_PORT";
        public const string BARCODE_COM_PORT = "BRIDGE_BARCODE_COM_PORT";
        public const string RFID_READ_DELAY = "BRIDGE_RFID_READ_DELAY";
        public const string BRIDGE_ID = "BRIDGE_BRIDGE_ID";
        public const string GIN_NAME = "BRIDGE_GIN_NAME";
        public const string LATITUDE = "BRIDGE_LATITUDE";
        public const string LONGITUDE = "BRIDGE_LONGITUDE";
        public const string TARGET_STATUS = "BRIDGE_TARGET_STATUS";
        public const string DOCUMENT_DB_KEY = "BRIDGE_DOCUMENT_DB_KEY";
        public const string DOCUMENTDB_ENDPOINT = "BRIDGE_DOCUMENTDB_ENDPOINT";
        public const string DATA_SYNC_INTERVAL = "BRIDGE_DATA_SYNC_INTERVAL";

        public const string LAST_SYNC_TIME = "BRIDGE_LAST_SYNC_TIME";

        public const string SYNC_RUNNING = "BRIDGE_SYNC_RUNNING";

        public const string STABLE_WEIGHT_SECONDS = "STABLE_WEIGHT_SECONDS";

        public const string WEIGH_IN_TIMEOUT = "WEIGH_IN_TIMEOUT";

        public const string READER_HOST_NAME = "READER_HOST_NAME";

        public const string UNATTENDED_MODE = "UNATTENDED_MODE";

        public const string WEIGHT_AUTO_SAVE_TIMEOUT = "WEIGHT_AUTO_SAVE_TIMEOUT";

    }

    public static class GinAppSettingKeys
    {
        public const string SETUP_WIZARD_COMPLETE = "SETUP_WIZARD_COMPLETE";
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

        public const string AZURE_DOCUMENTDB_READONLY_ENDPOINT = "AZURE_DOCUMENTDB_READONLY_ENDPOINT";
        public const string AZURE_DOCUMENTDB_READONLY_KEY = "AZURE_DOCUMENTDB_READONLY_KEY";

        public const string LOAD_PREFIX = "LOAD_PREFIX";
        public const string STARTING_LOAD_NUMBER = "STARTING_LOAD_NUMBER";
        public const string MODULES_PER_LOAD = "MODULES_PER_LOAD";
        public const string GIN_NAME = "GIN_NAME";

        public const string LAST_LOCAL_IMPORT_TIMESTAMP = "LAST_LOCAL_IMPORT_TIMESTAMP";
        public const string LAST_IMPORT_STATUS = "LAST_IMPORT_STATUS";
        public const string LAST_SYNC_TIME = "GIN_LAST_SYNC_TIME";
    }

    public class Setting
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
