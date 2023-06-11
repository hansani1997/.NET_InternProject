using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Models
{
    public class UnitComboData
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public int User { get; set; }
        public int Company { get; set; }
        public long RequestingElement { get;set; }
        public string url { get; set; } = "https://localhost:7036/api/Unit/ReadUnits";
        public long ItemKey { get; set; }
        public string unitResponses { get; set; }
    }
}
