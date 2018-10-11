//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CottonDBMS.GinApp.Helpers
{
    public static class FileHelper
    {
        public static string CleanFilename(string filename)
        {            
            return string.Join("", filename.Split(Path.GetInvalidFileNameChars()));
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
