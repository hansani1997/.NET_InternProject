using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Models
{
    public class StockAsAt
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public int User { get; set; }
        public int Company { get; set; }
        public int Project { get; set; }
        public long element { get; set; }
        public long Location { get; set; }
        public long ItemCd { get; set; }
        public decimal CurStk { get; set; }
        public bool isLocked { get; set; }
        public DateTime timestamp { get; set; }
    }
}
