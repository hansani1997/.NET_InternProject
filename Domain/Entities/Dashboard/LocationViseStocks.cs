using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard
{
    public  class LocationViseStockRequest:BaseEntity
    {
        public long ElementKey { get; set; } = 1;
        public DateTime? AsAtDate { get; set; }
        public ItemResponse Item { get; set; } 
        public CodeBaseResponse Location { get; set; }
        public decimal Vat { get; set; }
        public string Unit { get; set; } = "";
        public decimal VatInclusivePrice { get; set; }
        public decimal SalesPrice { get; set; }

        public LocationViseStockRequest()
        {
            Item= new ItemResponse();
            Location = new CodeBaseResponse();
            AsAtDate = DateTime.Now;
            VatInclusivePrice = 0;
            SalesPrice = 0;
            Vat = 15;

        }

        public void CopyFrom(LocationViseStockRequest source)
        {
            source.CopyProperties(this);

        }
    }


    public class LocationViseStockResponse
    {
        public CodeBaseResponse Location { get; set; }
        public decimal Qty { get; set; } 
        public UnitResponse Unit { get; set; }

        public decimal SalesPrice { get; set; }
        public decimal VATInclusivePrice { get; set; }
        public string Remarks { get; set; }
        public string SplInfo { get; set; }

        public LocationViseStockResponse()
        {
            this.Qty = 0;
            this.SalesPrice = 0;
            this.Location = new();
            this.Unit = new();
            this.VATInclusivePrice = 0;
            this.Remarks = String.Empty;
            this.SplInfo = String.Empty;
        }

        
    }
}
