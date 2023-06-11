using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Routes
{
    public static class ReportEndPoints
    {
        public static string ReportPinnedMenuURL = BaseEndpoint.BaseURL + "Report/reportPinnedUnpinnedMenu";
        public static string ReportModuleMenuURL = BaseEndpoint.BaseURL + "Report/reportModuleMenu";
        public static string ReportModuleSubMenuURL = BaseEndpoint.BaseURL + "Report/reportSubModuleMenu";
        public static string ReportPinnedUpdateURL = BaseEndpoint.BaseURL + "Report/updateReportPinnedUnpinnedMenu";
        public static string ReportFilterFieldsURL = BaseEndpoint.BaseURL + "Report/reportFilterFields";

    }
}
