using CottonDBMS.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.Bridges.Shared.Messages
{
    public enum ScaleMessageStatus { VALID, INVALID, MOTION, OVER_UNDER_RANGE }

    public class ScaleWeightReportMessage
    {
        public decimal Weight { get; private set; }
        public ScaleMessageStatus Status { get; private set; }
        public string Message { get; private set; }

        public ScaleWeightReportMessage(string message)
        {
            Message = message;

            //char stx = message[0];
            //char pol = message[1];
            char unit = ' ';
            char grossNet = 'G';
            char status = ' ';
            //char term = ' ';

            if (message.Length >= 9)
            {
                var weight = message.Substring(2, 7);
                decimal temp = 0.00M;
                if (decimal.TryParse(weight, out temp))
                    Weight = temp;
            }

            if (Weight > 0.00M)
            {
                Console.WriteLine("WEIGHT");
            }

            if (message.Length >= 10)
                unit = message[9];

            if (message.Length >= 11)
                grossNet = message[10];

            if (message.Length >= 12)
                status = message[11];


            if (status == ' ')
                Status = ScaleMessageStatus.VALID;
            else if (status == 'I')
                Status = ScaleMessageStatus.INVALID;
            else if (status == 'M')
                Status = ScaleMessageStatus.MOTION;
            else if (status == 'O')
                Status = ScaleMessageStatus.OVER_UNDER_RANGE;
            else
                Status = ScaleMessageStatus.INVALID;

        }

        public string StatusName
        {
            get
            {
                switch (Status)
                {
                    case ScaleMessageStatus.INVALID: return "INVALID";
                    case ScaleMessageStatus.MOTION: return "MOTION";
                    case ScaleMessageStatus.OVER_UNDER_RANGE: return "OUT OF RANGE";
                    case ScaleMessageStatus.VALID: return "VALID";
                    default:
                        return "";
                }
            }
        }
    }

    public class WeightAcquiredMessage
    {
        public decimal Weight { get; set; }
    }

    public class InMotionMessage
    {
        public decimal Weight { get; set; }
    }

    public class KeyDownMessage
    {
        public string Key { get; set; }
    }

    public class InactiveMessage
    {

    }

    public class LoadSavedMessage
    {
        public LoadScanEntity Scan { get; set; }
    }

}
