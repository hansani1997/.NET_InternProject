using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain.Settings
{
    public static class BLMathSettings
    {
        public static int DefaultPriceDecimalsRounding { get; set; } = 2;
        public static int DefaultQuantiyDecimalsRounding { get; set; } = 2;
        public static int DefaultDecimalsRounding { get; set; } = 2;
        public static string DateFormat  { get; set; } = "dd/MMM/yyyy";

        public static string NumberFormat { get; set; } = "N2";

        public static string QuantiyNumberFormat { get; set; } = "N2";
        public static string PriceNumberFormat { get; set; } = "N2";

        public static MidpointRounding  RoundingMode { get; set; } = MidpointRounding.AwayFromZero;



        public static decimal BLRound(decimal value)
        {
            return Math.Round(value, DefaultDecimalsRounding, RoundingMode);
        }



    }
}
