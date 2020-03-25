using CottonDBMS.DataModels;
using CottonDBMS.GinApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.GinApp.Classes
{
    public class ClassingFileImportTask : IDisposable
    {   
        private string[] _paths;

        public event ProgressHandler OnProgressUpdate;
        public EventArgs e = null;

        //private Dictionary<string, bool> existingSerialNumbers = new Dictionary<string, bool>(50000);

        int baleColIndex     = -1;
        int netWtColIndex    = -1;
        int loadColIndex     = -1;
        int producerColIndex = -1;
        int farmColIndex     = -1;
        int fieldColIndex    = -1;
        int pkColIndex       = -1;
        int grColIndex       = -1;
        int lfColIndex       = -1;
        int stColIndex       = -1;
        int micColIndex      = -1;
        int exColIndex       = -1;
        int rmColIndex       = -1;
        int strColIndex      = -1;

        int cgrColIndex = -1;

        int rdColIndex    = -1;
        int plusbColIndex = -1;
        int trColIndex    = -1;
        int unifColIndex  = -1;
        int lenColIndex   = -1;
        int valueColIndex = -1;
        
        //DateTime? lastStatusUpdate = null;

        /*private void updateStatus(string msg, bool forceUpdate)
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
        }*/

        private bool processRow(CSVRow row, CSVFileParser parser)
        {
            try
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    string baleNumber = row.GetColumnValue(baleColIndex);                    
                    var bale = uow.BalesRepository.FindSingle(x => x.PbiNumber.EndsWith(baleNumber));

                    if (bale != null)
                    {
                        bale.Classing_NetWeight = row.GetDecimalColumnValueNullabe(netWtColIndex);
                        bale.Classing_Pk = row.GetIntColumnValueNullabe(pkColIndex);
                        bale.Classing_Gr = row.GetIntColumnValueNullabe(grColIndex);
                        bale.Classing_Lf = row.GetIntColumnValueNullabe(lfColIndex);
                        bale.Classing_St = row.GetIntColumnValueNullabe(stColIndex);
                        bale.Classing_Mic = row.GetDecimalColumnValueNullabe(micColIndex);
                        bale.Classing_Ex = row.GetIntColumnValueNullabe(exColIndex);
                        bale.Classing_Rm = row.GetIntColumnValueNullabe(rmColIndex);
                        bale.Classing_Str = row.GetDecimalColumnValueNullabe(strColIndex);
                        bale.Classing_CGr = row.GetColumnValue(cgrColIndex);
                        bale.Classing_Rd = row.GetDecimalColumnValueNullabe(rdColIndex);
                        bale.Classing_Plusb = row.GetDecimalColumnValueNullabe(plusbColIndex);
                        bale.Classing_Tr = row.GetIntColumnValueNullabe(trColIndex);
                        bale.Classing_Unif = row.GetDecimalColumnValueNullabe(unifColIndex);
                        bale.Classing_Len = row.GetIntColumnValueNullabe(lenColIndex);
                        bale.Classing_Value = row.GetDecimalColumnValueNullabe(valueColIndex);
                        uow.BalesRepository.Save(bale);
                        uow.SaveChanges();
                    }  
                }

                return true;
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                return false;
            }
        }
              
        public int processFile(System.IO.FileInfo fileInfo)
        {
            int errorCount = 0;
            try
            {
                int readTries = 0;
                bool canReadFile = false;

                while (readTries < 20 && !canReadFile)
                {
                    if (!FileHelper.CanReadFile(fileInfo))
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
                    baleColIndex     = parser.Header.FindColumnIndex(new string[] { "Bale"});
                    netWtColIndex    = parser.Header.FindColumnIndex(new string[] { "NetWt", "Net Wt" });
                    loadColIndex     = parser.Header.FindColumnIndex(new string[] { "Load" });
                    producerColIndex = parser.Header.FindColumnIndex(new string[] { "Prod ID" });
                    farmColIndex     = parser.Header.FindColumnIndex(new string[] { "Farm ID"});
                    fieldColIndex    = parser.Header.FindColumnIndex(new string[] { "Field ID" });
                    pkColIndex       = parser.Header.FindColumnIndex(new string[] { "Pk" });
                    grColIndex       = parser.Header.FindColumnIndex(new string[] { "Gr" });
                    lfColIndex       = parser.Header.FindColumnIndex(new string[] { "Lf" });
                    stColIndex       = parser.Header.FindColumnIndex(new string[] { "St" });
                    micColIndex      = parser.Header.FindColumnIndex(new string[] { "Mic" });
                    exColIndex       = parser.Header.FindColumnIndex(new string[] { "Ex" });
                    rmColIndex       = parser.Header.FindColumnIndex(new string[] { "Rm" });
                    strColIndex      = parser.Header.FindColumnIndex(new string[] { "Str" });
                    cgrColIndex      = parser.Header.FindColumnIndex(new string[] { "CGr" });
                    rdColIndex       = parser.Header.FindColumnIndex(new string[] { "Rd" });
                    plusbColIndex    = parser.Header.FindColumnIndex(new string[] { "plusb" });
                    trColIndex       = parser.Header.FindColumnIndex(new string[] { "Tr" });
                    unifColIndex     = parser.Header.FindColumnIndex(new string[] { "Unif" });
                    lenColIndex      = parser.Header.FindColumnIndex(new string[] { "Len" });
                    valueColIndex    = parser.Header.FindColumnIndex(new string[] { "Value" });                                      

                    int totalRows = parser.Rows.Count();
                    foreach (var row in parser.Rows)
                    {
                        //updateStatus(string.Format("Processing file {0} of {1} - row {2} of {3}.", fileNumber, totalFiles, rowCount, totalRows), false);
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

        public ClassingFileImportTask()
        {
            
        }        

        public void Dispose()
        {

        }
    }
}
