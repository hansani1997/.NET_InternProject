using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Models
{
    public class AddressComboModel
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public int User { get; set; }
        public int Company { get; set; }
        public long RequestingElement { get; set; }
        public long AddressKey { get; set; }
        //public string AddressCode { get; set; }
        public string addressMasterObject { get; set; }
        public string AddressName { get; set; }
        public string AddressObject { get; set; }
        public bool isNew { get; set; } = false;
        public bool isPushed { get; set; } = true;
        public DateTime timestamp { get; set; }
    }
}
