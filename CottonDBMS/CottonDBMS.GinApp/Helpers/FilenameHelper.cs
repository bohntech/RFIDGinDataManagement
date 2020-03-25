//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace CottonDBMS.GinApp.Helpers
{
    public static class FileHelper
    {

        public static bool CanReadFile(FileInfo file)
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

        public static string CleanFilename(string filename)
        {            
            return string.Join("", filename.Split(Path.GetInvalidFileNameChars()));
        }

        public static void ToCSV(this DataTable dtDataTable, string strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath, false);
            //headers  
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {                        
                        string value = EscapeForCSV(dr[i].ToString());
                        sw.Write(value);
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }

        public static string EscapeForCSV(string value)
        {
            if (string.IsNullOrEmpty(value)) value = "";

            string escapedVal = "";
            if (value.Contains("\""))
            {
                escapedVal = value.Replace("\"", "\"\"");
            }

            if (escapedVal == "")
            {
                escapedVal = value;
            }

            if (escapedVal.Contains(","))
            {
                escapedVal = "\"" + escapedVal + "\"";
            }           

            return escapedVal;
        }
    }
}
