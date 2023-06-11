using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain.DTO.Report
{
    public class ReportConversion
    {
        public string HtmlRaw { get; set; }
        public string Base64Cotent { get; set; }
    }

    public class ReportCompanyDetailsRequest
    {
        public CodeBaseResponse BussinessUnit { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Location { get; set; } = new CodeBaseResponse();
        public long TransactionKey { get; set; } = 1;

        public long OrderKey { get; set; } = 1;
        public long EmployeeKey { get; set; }


    }
    public class ReportCompanyDetailsResponse
    {

        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }

        public string Town { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string TP1 { get; set; }

        public string TP2 { get; set; }
        public string TP3 { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string TaxNo2 { get; set; }
        public string EPFRegNo { get; set; }
        public string b64Logo { get; set; }
        public string BRNo { get; set; }
        public string TaxNo { get; set; }
        public string BOIRegNo { get; set; }
        public string Remarks { get; set; }





    }
}
