using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.HRManager
{
    public interface IHRManager : IManager
    {
        bool IsExceptionthrown();
        Task<UserDetails> GetUserAsync();
        Task<UserDetails> GetAddressDetailsBylogin();
        Task<IList<MultiAtnAnlysis_Response>> GetExistingRecordForDay(MultiAtnAnlysis attendence);
        Task<IList<MultiAtnAnlysis_Response>> GetExistingRecordForDayV2(MultiAtnAnlysis attendence);
        Task<InShift> GetShift(ManualAttendence req);
        Task<MultiAtnAnlysis_Response> InOut(ManualAttendence attendence);
        Task UpdateRecord(UpdateAttendence attendence);
        Task<MultiAtnAnlysis_Response> CreateManualAttendence(AddManualAdt attendence);
        Task<UserPermission> GetUserPermission(UserPermission obj);

        //self leave request 
        Task<IList<LeaveDetails>> GetAlreadyAppliedLeaves();
        Task<int> GetLeaveTrnTypeDetails(LeaveTrnTypeDetails slh);
        Task<UserDetails> GetReportingPerson(UserDetails req);
        Task<decimal> GetLeaveTypeByCompany(UserDetails usr);
        Task<int> GetMultiApproval(MultiApprovalDetails multiApproval);
        Task<IList<LeaveSummary>> LoadLeaveSummary(LeaveDetails levDet);
        Task ApplyLeave(Leaverequest req);
        Task<int> SelectLeaveCheck(LeaveCheck req);
        Task DeleteLeave(LeaveDetails req);

        Task<IList<LeaveDetails>> LeaveFilter(Leaverequest req);
        Task ChangeLeaveStatus(LeaveStatusChange req);
        //end

        Task<EmployeeModel> LoadEmployeeDetails();
        Task<IList<PaySlipDetails>> GeneratePaySlip(SalaryHistory req);
        Task<IList<TaskwiseAttendance>> TaskwiseAttendance(InRequest request);
        Task<bool> SaveTaskwiseAttendance(IList<TaskwiseAttendance> request);
    }
}
