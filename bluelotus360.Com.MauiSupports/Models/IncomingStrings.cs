using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Models
{
    public class IncomingStrings
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }
        public string name { get; set; }
        public string parameters { get; set; }
        //public string UnfilteredParameters { get; set; } = "";
        public string response { get; set; }
        public int user { get; set; }
        public int company { get; set; }
        public DateTime timestamp { get; set; }
    }
}
