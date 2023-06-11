using BL10.CleanArchitecture.Domain.Entities.Dashboard;
using BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.HRAdminDashboardManager
{
    public interface IHRAdminDashboardManager : IManager
    {
        bool IsExceptionthrown();
        //Task<IResult<DashboardDataResponse>> GetDataAsync();

        Task<HrAdminDashboardChartResponse> GetLocationWiseHeadCount(HRAdminDashboardRequest request);
        Task<HRAdminDashboardChart2Response> GetAttendanceSummary(HRAdminDashboardRequest request);
        Task<HRAdminDashboardCardResponse> GetHRAdminDashboardCardDetails(HRAdminDashboardCardRequest request);
       
        Task<HRAdminTaskwiseAttendanceActualResponse> GetTaskWiseAttendance(HRAdminDashboardRequest request);
      
    }
}
