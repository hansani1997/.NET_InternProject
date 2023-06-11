using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.Dashboard
{
    
    public class SalesAndProductivityFilter
    {
        public AddressResponse SalesRepresentative { get; set; }
        public DateTime? FromDate { get; set; } = DateTime.Now;
        public DateTime? ToDate { get; set; } = DateTime.Now;
        public CodeBaseResponse AddressCategory3 { get; set; }
        public CodeBaseResponse AddressCategory1 { get; set; }
        public int? ItemCategory3Key { get; set; } = 0;

        public SalesAndProductivityFilter() {
            SalesRepresentative = new AddressResponse();
            AddressCategory3 = new CodeBaseResponse();
            AddressCategory1 = new CodeBaseResponse();
        }
    }
   
    public class SummaryInfo
    {
        public int RouteKey { get; set; }
        public string RouteOrShop { get; set; }
        public int VisitsPlanned { get; set; }
        public int VisitsDone { get; set; }
        public int Orders { get; set; }
        public int Invoice { get; set; }
    }
    public class RouteSummary
    {
        public IEnumerable<SummaryInfo> Info { get; set; }
    }
    public class ShopsForRoute
    {
        public IEnumerable<SummaryInfo> Info { get; set; }
    }

    public class BillCoverage
    {
        public decimal Target { get; set; }
        public decimal Achievement { get; set; }
        public decimal PercentageCovered { get; set; }
    }
    public class BillProductivity
    {
        public decimal BillCount { get; set; }
        public decimal VisitCount { get; set; }
        public decimal PercentageCovered { get; set; }
    }

    public class OverallSales
    {
        public decimal SalesValue { get; set; }
        public decimal SalesTargetValue { get; set; }
        public decimal AchievedPercentage { get; set; }
        public decimal NonAchievedPercentage { get; set; }
        public decimal CashPercentage { get; set; }
        public decimal CreditPercentage { get; set; }
        public decimal CashSalesValue { get; set; }
        public decimal CreditSalesValue { get; set; }
    }
    public class ItemCategoryDetails
    {
        public int CategoryKey { get; set; }
        public String CategoryName { get; set; }
        public decimal SalesValue { get; set; }
        public decimal TargetValue { get; set; }
        public decimal TargetQty { get; set; }
        public decimal AchievedPercentage { get; set; }
        public decimal NonAchievedPercentage { get; set; }
        public List<SubCategory> SubCategoryList { get; set; }
        public ItemCategoryDetails()
        {

        }
    }
    public class SubCategory
    {
        public int SubCategoryKey { get; set; }
        public String SubCategoryName { get; set; }
        public decimal SalesQty { get; set; }
    }

    public class SalesAndProductivityCombinedModelForMainCharts
    {
        public BillCoverage BillCoverage { get; set; }
        public BillProductivity BillProductivity { get; set; }
        public OverallSales OverallSales { get; set; }  
        public Dictionary<int,string> MainCategories { get; set; }
    }

    // The modals that will be used to bring in the responses will be SummaryInfo, RouteSummary, ShopSummary, SalesAndProductivityCombinedModelForMainCharts, ItemCategoryDetails
    // These will be the 4 reponse objects used to get the data into the charts when required.
    
//    public class SampleData
//    {
//        //Sample data class to assist with testing the Sales and Productivity Dashboard UI
//        public static SummaryInfo GetSummaryInfo() {
//            return new SummaryInfo()
//            {
//                Route="",
//                VisitsPlanned = 8,
//                VisitsDone = 3,
//                Orders = 2,
//                Invoice = 1
//            };
//        }
//        public static RouteSummary GetSummaryTable() {
//            return new RouteSummary() 
//            {
//                Info = new List<SummaryInfo>()
//                {
//                    new SummaryInfo() {Route=  "Route 1", VisitsPlanned = 2,VisitsDone = 1,Orders = 1,Invoice = 1},
//                    new SummaryInfo() {Route = "Route 2",VisitsPlanned = 4,VisitsDone = 4,Orders = 0,Invoice = 0},
//                    new SummaryInfo() {Route = "Route 3", VisitsPlanned = 2,VisitsDone = 2,Orders = 0,Invoice = 0},
//                    new SummaryInfo() {Route = "Route 4", VisitsPlanned = 3,VisitsDone = 2,Orders = 1,Invoice = 1},
//                    new SummaryInfo() {Route="Athula Hardware", VisitsPlanned = 2,VisitsDone = 1,Orders = 1,Invoice = 1},
//                    new SummaryInfo() {Route = "Dishan Trade Center",VisitsPlanned = 4,VisitsDone = 4,Orders = 0,Invoice = 0},
//                    new SummaryInfo() {Route = "Dimuthu Building Material", VisitsPlanned = 2,VisitsDone = 2,Orders = 0,Invoice = 0},
//                    new SummaryInfo() {Route = "Nithika Stores", VisitsPlanned = 3,VisitsDone = 2,Orders = 1,Invoice = 1},
//                    new SummaryInfo() {Route="Athula Hardware", VisitsPlanned = 2,VisitsDone = 1,Orders = 1,Invoice = 1},
//                    new SummaryInfo() {Route = "Dishan Trade Center",VisitsPlanned = 4,VisitsDone = 4,Orders = 0,Invoice = 0},
//                    new SummaryInfo() {Route = "Dimuthu Building Material", VisitsPlanned = 2,VisitsDone = 2,Orders = 0,Invoice = 0},
//                    new SummaryInfo() {Route = "Nithika Stores", VisitsPlanned = 3,VisitsDone = 2,Orders = 1,Invoice = 1}
//                }
//            };
//        }
        
//        public static BillCoverage GetBillCoverage()
//        {
//            return new BillCoverage() { PercentageCovered = 80.01M };
//        }
//        public static BillProductivity GetBillProductivity()
//        {
//            return new BillProductivity() { PercentageCovered = 25.71M };
//        }
//        public static OverallSales GetOverallSales()
//        {
//            return new OverallSales()
//            {
//                SalesValue = 185000,
//                SalesTargetValue = 250000,
//                AchievedPercentage = 25.6M,
//                NonAchievedPercentage = 74.4M,
//                CashPercentage = 65,
//                CreditPercentage = 35
//            }; 
//        }
//        public static ItemCategoryDetails GetItemCategoryDetails()
//        {
//            return new ItemCategoryDetails()
//            {
//                CategoryKey= 12344,
//                CategoryName= "Adhesive",
//                SalesValue= 86500,
//                TargetValue = 9000,
//                TargetQty = 30,
//                AchievedPercentage = 34.5M,
//                NonAchievedPercentage = 100-34.5M,
//                SubCategoryList = new List<SubCategory> { 
//                    new SubCategory(){ SubCategoryKey=23434,SubCategoryName="Super",SalesQty=50},
//                    new SubCategory(){ SubCategoryKey=21333,SubCategoryName="Super Plus",SalesQty=100},
//                    new SubCategory(){ SubCategoryKey=23494,SubCategoryName="Ultra Grip",SalesQty=10}
//                }
//            };
//        }
//    }
    
}
