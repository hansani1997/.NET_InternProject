using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Application.Validators.Report
{
    public class ReportDashboardValidator: IReportValidator
    {
        public IDictionary<string, object> ReportParameters { get; set; }
        public UserMessageManager UserMessages { get; set; }
        public IList<string> RequiredElements { get; set; }
        public ReportDashboardValidator(BLUIElement uielement, IDictionary<string, object> reportParameters)
        {
            ReportParameters = reportParameters;
            RequiredElements = uielement.IsMustElements;
            UserMessages = new UserMessageManager();
        }

        public bool CanSelectItem()
        {
            UserMessages.UserMessages.Clear();

            foreach(KeyValuePair<string, object> reportPara in ReportParameters) //ky>11 or nm/cd/id not null or empty
            {
                bool validCheck = true;
                BaseResponse response;

                if (RequiredElements.Contains(reportPara.Key))
                {
                    if (reportPara.Key.EndsWith("Ky", StringComparison.OrdinalIgnoreCase) && (Convert.ToInt32(reportPara.Value) < 11))
                    {

                        UserMessages.AddErrorMessage("Please fill " + reportPara.Key);
                    }
                    if (reportPara.Key.EndsWith("Nm", StringComparison.OrdinalIgnoreCase) && (string.IsNullOrEmpty(reportPara.Value.ToString())))
                    {
                        UserMessages.AddErrorMessage("Please fill " + reportPara.Key);
                    }
                    if (reportPara.Key.EndsWith("Cd", StringComparison.OrdinalIgnoreCase) && (string.IsNullOrEmpty(reportPara.Value.ToString())))
                    {
                        UserMessages.AddErrorMessage("Please fill " + reportPara.Key);
                    }
                    if (reportPara.Key.EndsWith("Id", StringComparison.OrdinalIgnoreCase) && (string.IsNullOrEmpty(reportPara.Value.ToString())))
                    {
                        UserMessages.AddErrorMessage("Please fill " + reportPara.Key);
                    }
                }
              
                

                //if ( && !validCheck)
                //{
                //    UserMessages.AddErrorMessage("Please fill "+ reportPara.Key);
                //}
            }



           

            return UserMessages.UserMessages.Count == 0;
        }
    }

    
}
