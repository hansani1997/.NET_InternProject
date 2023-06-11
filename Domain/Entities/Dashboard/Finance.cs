using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard
{
   
        public class FinanceRequest
        {
            public long ElementKey { get; set; } = 1;
            public DateTime? FromDate { get; set; }=DateTime.Now;
            public DateTime? ToDate { get; set; } = DateTime.Now;
            public CodeBaseResponse BusinessUnit { get; set; }

            public FinanceRequest()
            {
                BusinessUnit = new CodeBaseResponse() ;
            }

            
        }

        public class FinanceRequestDT : FinanceRequest
        {
            public int DayS { get; set; }
            public int DayE { get; set; }
        }

        public class FinanceRequestDTDetails : FinanceRequestDT
        {
            public string AccKy { get; set; } = "";
            public string AccType { get; set; } = "";
        }

    public class ActualVsBudgetedIncomeResponse
        {
            public string YYYY_MM { get; set; } = "";
            public double Income { get; set; }

            public int BgtIncome { get; set; }
            public int BgtExpence { get; set; }
        }

    public class GPft_NetPft_Margin_Response
    {
        public double Gnm { get; set; }

        public string XAx { get; set; } = "";

        public GPft_NetPft_Margin_Response(double gnm, string x)
        {
            Gnm = gnm;
            XAx = x;
        }
    }

    public class GPft_NetPft_DT 
    {
        public double Revenue { get; set; }
        public double CostOfSale { get; set; }
        public double GrsProfit { get; set; }
        public double GpMargin { get; set; }
        public double Income { get; set; }
        public double Expence { get; set; }
        public double NetProfit { get; set; }
        public double NPMargin { get; set; }
        public string YYYY_MM { get; set; } = "";
    }


    public class Debtors_Creditors_Age_Analysis
    {
        public double Amt { get; set; }
        public string Hdr { get; set; } = "";
        public int DayS { get; set; }
        public int DayE { get; set; }
    }

    public class Debtors_Creditors_Age_Analysis_DT : Debtors_Creditors_Age_Analysis
    {
        public string acccd { get; set; } = "";
        public string accnm { get; set; } = "";
        public string AccKy { get; set; } = "";
        public string AccountType { get; set; } = "";

    }

    public class Debtors_Creditors_Transaction_Details
    {
        public int TrnKy { get; set; }
        public string TrnDt { get; set; } = "";
        public string TrnTyp { get; set; } = "";
        public string TrnNo { get; set; } = "";
        public double TrnAmt { get; set; }
        public double DueAmt { get; set; }
        public int ObjKy { get; set; } = 1;
    }

    public class FinanceChartCombinedModel
    {
        public IList<ActualVsBudgetedIncomeResponse> ActualVsBudgetedIncomceResponse { get; set; }
        public IList<GPft_NetPft_Margin_Response> GPft_NetPft_Margin_Response { get; set; }
        public IList<GPft_NetPft_DT> GPft_NetPft_Margi_DT_Response { get; set; }
        public IList<Debtors_Creditors_Age_Analysis> Debtors_Age_Analysis_Response { get; set; }
        public IList<Debtors_Creditors_Age_Analysis> Creditors_Age_Analysis_Response { get; set; }
        public IList<Debtors_Creditors_Age_Analysis> Debtors_Age_Overdue_Response { get; set; }
        public IList<Debtors_Creditors_Age_Analysis> Creditors_Age_Overdue_Response { get; set; }
    }

}
