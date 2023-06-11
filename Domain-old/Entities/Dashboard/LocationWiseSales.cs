using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard
{
    public class SalesByLocation
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public CodeBaseResponse BusinessUnit { get; set; } 
        public CodeBaseResponse Location { get; set; }

        public SalesByLocation()
        {
            FromDate=DateTime.Now;
            ToDate=DateTime.Now;
            BusinessUnit = new CodeBaseResponse();
            Location = new CodeBaseResponse();  
        }
    }

    public class SalesByLocationResponse
    {
        public CodeBaseResponse Location { get; set; }
        public decimal TotalSalesAmt { get; set; } = 0;

        public SalesByLocationResponse(CodeBaseResponse location,decimal total_sales_amount)
        {
            Location = location;
            TotalSalesAmt = total_sales_amount;
        }
    }
    public class SalesDetails: SalesByLocation
    {
        public long ElementKey { get; set; } 
        public int RepAdrKy { get; set; }
        public long RefId { get; set; }
        public decimal CashAmt { get; set; } = 0.0M;
        public decimal TotalCashAmt { get; set; } = 0.0M;
        public decimal CardAmt { get; set; } = 0.0M;
        public decimal TotalCardAmt { get; set; } = 0.0M;
        public decimal OtherAmt { get; set; } = 0.0M;
        public decimal TotalReturn { get; set; } = 0.0M;
        public decimal TotalPaidOut { get; set; } = 0.0M;
        public int TotalNewCustomers { get; set; } = 0;
        public decimal TotalPaidByCustomers { get; set; } = 0.0M;
        public decimal TotalSalesAmt { get; set; } = 0.0M;
        public string TrnNo { get; set; }
        public DateTime InsertDate { get; set; }


        public SalesDetails(){
            Location = new CodeBaseResponse();
            BusinessUnit = new CodeBaseResponse();
            FromDate=DateTime.Now;
            ToDate=DateTime.Now;
            RepAdrKy = 1;
            ElementKey = 1;
        }
    }


    public class SalesRepDetailsForSalesByLocation : SalesDetails
    {
        public int TrnTypKy { get; set; } = 1;
        public int AccKy { get; set; } = 1;
        public int OurCordinatorKy { get; set; } = 1;
        public int AdrCat3Ky { get; set; } = 1;

    }

    public class SalesRepDetailsForSalesByLocationResponse
    {
        public string RepAdrName { get; set; }
        public decimal GrsAmt { get; set; }
        public decimal DisAmt { get; set; }
        public decimal Total { get; set; }

    }

}
