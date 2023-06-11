using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Models
{
    public class ItemComboResponses
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; } //primary
        public long ObjectKey { get; set; } //unique filter
        public int company { get; set; } //used for saving
        public int user {  get; set; } //used for saving
        public long ItemKey { get; set; }
        public string ItemName { get; set; } //User for search
        public string ItemObject { get; set; }
        public DateTime timestamp { get; set; }
    }
}
