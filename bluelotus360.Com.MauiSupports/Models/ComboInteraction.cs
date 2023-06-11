using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Models
{
    public class ComboInteraction
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string internalElementName { get; set; }
        public string eventName { get; set; }
        public string eventAction { get; set; }
        public DateTime eventStart { get; set; }
        public DateTime eventEnd { get; set; }
    }
}
