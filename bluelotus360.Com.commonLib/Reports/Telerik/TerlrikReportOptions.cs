using bluelotus360.Com.commonLib.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Reports.Telerik
{
    public  class TerlrikReportOptions
    {
        public string ReportName { get; set; }
        public string ReportRestServicePath { get;protected set; }

        public IDictionary<string, object> ReportParameters { get; set; }

        public TerlrikReportOptions()
        {
            this.ReportRestServicePath = BaseEndpoint.ReportURL;
        }
    }
}
