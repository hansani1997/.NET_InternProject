using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BL10.CleanArchitecture.Domain.Entities.Dashboard
{
    public class HRAdminDashboardCardRequest
    {
        public DateTime? Date { get; set; }

       public HRAdminDashboardCardRequest()
        {
            Date = DateTime.Now;
        }
    }

    public class HRAdminDashboardCardResponse
    {
        public string? AddressID { get; set; } = "";
        public string? AddressName { get; set; } = "";
        public string? CodeName { get; set; } = "";
        public string? DOB { get; set; } = "";
        public string? EmployeeNo { get; set; } = "";
        public string? EmployeeName { get; set; } = "";
        public string? JoinDate { get; set; } = "";
        public string? ToBePermanent { get; set; } = "";
        public string? EffectiveDate { get; set; } = "";

        public int TotalBirthdayCount { get; set; }
        public int TotalProbationToPermanentEmployeeCount { get; set; }
        public int TotalNewRecruitmentCount { get; set; }
        public int TotalLeftEmployeeCount { get; set; }
    }

    public class HRAdminDashboardRequest 
    {
        public DateTime? Date { get; set; }
        public CodeBaseResponse EmployeeType { get; set; } = new CodeBaseResponse();
        public int EmployeeKey { get; set; } = 1;
        public int BusinessUnit { get; set; } = 1;
        public int AddressKy { get; set; } = 1;
        public int ProjectKey { get; set; } = 1;
        public int ObjectKey { get; set; } = 1;
        public int ProcessDetailKey { get; set; } = 1;

        public HRAdminDashboardRequest()
        {
            Date = DateTime.Now;
            EmployeeType = new CodeBaseResponse();
        }

    }

    public class HRAdminDashboardHeadCountResponse
    {
        public int Count { get; set; }
        public int CodeKey { get; set; } = 1;
        public string? CodeName { get; set; } = "";

        public HRAdminDashboardHeadCountResponse()
        {
            Count = 0;
        }
    }

    public class HRAdminAttendanceSummaryResponse
    {
        public int Count { get; set; }
        public string? Status { get; set; } = "";
        public int CodeKey { get; set; } = 1;

        public HRAdminAttendanceSummaryResponse()
        {
            Count = 0;
        }
    }
    public class HrAdminDashboardChartResponse
    {
        public IList<HRAdminDashboardHeadCountResponse> MaleList { get; set; }
        public IList<HRAdminDashboardHeadCountResponse> FemaleList { get; set; }
        public HrAdminDashboardChartResponse()
        {
            MaleList = new List<HRAdminDashboardHeadCountResponse>();
            FemaleList = new List<HRAdminDashboardHeadCountResponse>();
        }
    }

    public class HRAdminDashboardChart2Response
    {
        public IList<HRAdminAttendanceSummaryResponse> OnTimeList { get; set; }
        public IList<HRAdminAttendanceSummaryResponse> LateList { get; set; }
        public IList<HRAdminAttendanceSummaryResponse> LeaveList { get; set; }
        public IList<HRAdminAttendanceSummaryResponse> NotReportedList { get; set; }

        public HRAdminDashboardChart2Response()
        {
            OnTimeList = new List<HRAdminAttendanceSummaryResponse>();
            LateList = new List<HRAdminAttendanceSummaryResponse>();
            LeaveList = new List<HRAdminAttendanceSummaryResponse>();
            NotReportedList = new List<HRAdminAttendanceSummaryResponse>();
        }
    }


    public class HRAdminTaskwiseA_ActualResponse
    {
        public string? QuantityTime { get; set; } = "";
        public string? EffectiveDate { get; set; } = "";
        public int CountOfEmployee { get; set; }
        public string? TotalHours { get; set; } = "";
        public string? AttendanceDate { get; set; } = "";
        public string? ActualHours { get; set; } = "";

        public HRAdminTaskwiseA_ActualResponse()
        {
            CountOfEmployee = 0;
        }
    }

    public class HRAdminTaskwiseAttendanceActualResponse
    {
        public IList<HRAdminTaskwiseA_ActualResponse> TaskwiseList { get; set; }
        public IList<HRAdminTaskwiseA_ActualResponse> AttendanceList { get; set; }
        public IList<HRAdminTaskwiseA_ActualResponse> ActualList { get; set; }

        public HRAdminTaskwiseAttendanceActualResponse()
        {
            TaskwiseList = new List<HRAdminTaskwiseA_ActualResponse>();
            AttendanceList = new List<HRAdminTaskwiseA_ActualResponse>();
            ActualList = new List<HRAdminTaskwiseA_ActualResponse>();
        }
    }
}
