using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain.DTO.Report
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class TrasnsactionReportLineItem
    {
        public decimal Quantity { get; set; }
        public decimal LineTotal { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal TransactionRate { get; set; }
        public long ItemKey { get; set; }
        public decimal LineDiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
    }

    public class TransactionReportLocal
    {
        public long TransactionKey { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal CashRecvAmt1 { get; set; }
        public decimal CashAmt2 { get; set; }
        public decimal CardAmt3 { get; set; }
        public decimal BalanceAmt6 { get; set; }
        public List<TrasnsactionReportLineItem> LineItems { get; set; } = new List<TrasnsactionReportLineItem>();
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string LocStreet { get; set; }
        public string LocCity { get; set; }
        public string LocBusinessPhone { get; set; }
        public string LocBusinessEmail { get; set; }
        public string TrasnsactionNumber { get; set; }
        public string TaxTyp { get; set; }
        public string Customer { get; set; }
        public string CustomerAddress { get; set; }
        public string EntUsrId { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalVAT { get; set; }
        public string PmtMode { get; set; }
        public string RepID { get; set; }
    }


}
