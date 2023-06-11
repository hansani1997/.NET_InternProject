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

        bool CanAddToGridServiceItem();
        bool CanAddToGridMaterialItem();
        bool CanAddToGridMiscellaneousItem();
        bool CanAddToGridMaterialItemInEstimate();
        bool CanAddToGridServiceItemInEstimate();
        bool CanAddQtyInEstimate();
        bool CanCreateIRNOrder();
        bool CanSaveWorkOrder();
        bool CanSaveTransaction();
    }
}
