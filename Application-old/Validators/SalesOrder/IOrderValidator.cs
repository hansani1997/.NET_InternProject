using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Application.Validators.SalesOrder
{
    public interface IOrderValidator
    {
        bool CanAddItemToGrid();
        bool CanRequestToAddItem();
        bool CanChangeHeaderInformatiom();
        bool CanOrderSave();
        bool CanSelectItem();
        UserMessageManager UserMessages { get; set; }


    }
}
