using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Application.Validators.HR
{
    public class HRValidator : IHRValidator
    {
        private AddManualAdt _time;

        public UserMessageManager UserMessages { get; set; }

        public HRValidator(AddManualAdt time)
        {
            UserMessages = new UserMessageManager();
            _time = time;
        }

        public bool CanAddTimeToGrid()
        {
            UserMessages.UserMessages.Clear();
            if (_time.InDtm != null && _time.OutDtm != null)
            {
                if (DateTime.Compare(_time.InDtm.Value, _time.OutDtm.Value) > 0)
                {
                    UserMessages.AddErrorMessage("In Time should be less than Out Time");
                }

            }
            if (_time.InDtm == null && _time.OutDtm == null)
            {
                UserMessages.AddErrorMessage("Please add in time or out time");
            }

            return UserMessages.UserMessages.Count == 0;
        }


    }

    public class EditAttendenceValidator : IHREditFormValidator
    {
        private UpdateAttendence _time;

        public UserMessageManager UserMessages { get; set; }

        public EditAttendenceValidator(UpdateAttendence time)
        {
            _time = time;
            UserMessages = new UserMessageManager();
        }
        public bool CanEditTimeToGrid()
        {
            UserMessages.UserMessages.Clear();
            if (_time.InDtm != null && _time.OutDtm != null)
            {
                if (DateTime.Compare(_time.InDtm.Value, _time.OutDtm.Value) > 0)
                {
                    UserMessages.AddErrorMessage("In Time should be less than Out Time");
                }
            }



            return UserMessages.UserMessages.Count == 0;
        }


    }

    public class LeaveRequestValidator : ILeaveRequestValidator
    {
        private Leaverequest _leave;

        public UserMessageManager UserMessages { get; set; }

        public LeaveRequestValidator(Leaverequest leave)
        {
            _leave = leave;
            UserMessages = new UserMessageManager();
        }
        public bool CanApplyLeave()
        {
            UserMessages.UserMessages.Clear();

            if (_leave.LeaveType.CodeKey == 1)
            {
                UserMessages.AddErrorMessage("Leave type can't be empty");

            }
            if (_leave.LevReason.CodeKey == 1)
            {
                UserMessages.AddErrorMessage("Leave reason can't be empty");

            }

            if (_leave.ToD != null && _leave.EftvDt != null)
            {
                if (DateTime.Compare(new DateTime(_leave.EftvDt.Value.Year, _leave.EftvDt.Value.Month, _leave.EftvDt.Value.Day), new DateTime(_leave.ToD.Value.Year, _leave.ToD.Value.Month, _leave.ToD.Value.Day)) > 0)
                {
                    UserMessages.AddErrorMessage("From date should be less than To date");
                }

                //if (_leave.EftvDt.Value.Year!= _leave.ToD.Value.Year)
                //{
                //    UserMessages.AddErrorMessage("From date and To date should be included in a same year");
                //}


            }

            if (_leave.MaxLeaveHour < _leave.ShortLeaveHours)
            {
                UserMessages.AddErrorMessage(" can’t apply more than " + _leave.MaxLeaveHour + " hours ");

            }

            int d = _leave.NoOfLeaveDays();
            if (_leave.LeaveType.CodeName == "Short - Short Leave")
            {
                if (_leave.MaxLeaveHour >= _leave.ShortLeaveHours && d > 2)
                {
                    UserMessages.AddErrorMessage("You cannot apply short leave for multiple days");
                }
            }


            return UserMessages.UserMessages.Count == 0;
        }

        public bool HasEntitlement()
        {
            UserMessages.UserMessages.Clear();
            bool flag = false;

            if (_leave.LeaveSummary.Count() == 0) { UserMessages.AddErrorMessage("Leave Entitlement is not found \n This Leave will be considered as No Pay"); }

            return UserMessages.UserMessages.Count == 0;
        }


    }

    public class PendingLeaveRequestValidator: IPendingLeaveRequestValidator
    {
        LeaveStatusChange _statuschange;
        public UserMessageManager UserMessages { get; set; }

        public PendingLeaveRequestValidator(LeaveStatusChange statuschange)
        {
            _statuschange = statuschange;
            UserMessages = new UserMessageManager();

            
        }

        public bool CanApproveLeave()
        {
            UserMessages.UserMessages.Clear();
            if (_statuschange.NextApprovedStatus.CodeKey == 1)
            {
                UserMessages.AddErrorMessage("Next Status can't be empty");

            }
            return UserMessages.UserMessages.Count == 0;
        }

    }

}
