using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Models
{
    public class RequestQueueItem
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }
        [NotNull]
        public int User { get; set; }
        public int Company { get; set; }
        public string url { get; set; }
        public string requestBody { get; set; }
        public string otherItemsSaved { get; set; }
        [NotNull]
        public string timeStamp { get; set; }
        [NotNull]
        public int isSynced { get; set; } = 0;
    }
}
