using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Application.Validators.WorkShopManagement
{
    public interface IInsuranceModuleValidator
    {
        UserMessageManager UserMessages { get; set; }

        bool CanCreateIRNOrder();
        bool CanAddToGridItem();
        bool CanSaveWorkOrder();
    }
}
