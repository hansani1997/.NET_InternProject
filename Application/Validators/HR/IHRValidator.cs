using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Application.Validators.HR
{
    public interface IHRValidator
    {
        bool CanAddTimeToGrid();
        
        UserMessageManager UserMessages { get; set; }
    }

    public interface IHREditFormValidator
    {
        bool CanEditTimeToGrid();
        UserMessageManager UserMessages { get; set; }
    }

    public interface ILeaveRequestValidator
    {
        bool CanApplyLeave();
        bool HasEntitlement();
        UserMessageManager UserMessages { get; set; }
    }

    public interface IPendingLeaveRequestValidator
    {
        bool CanApproveLeave();
        UserMessageManager UserMessages { get; set; }
    }
}
